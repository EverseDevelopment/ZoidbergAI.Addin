using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using Python.Runtime;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace Common.Handlers
{
    public class CodeExecutionHandler
    {
        internal UIApplication Application { get; set; }
        internal Document Doc { get; set; }
        internal int MaxRetries { get; set; }
        internal string Prompt { get; set; }
        internal string Code { get; set; }
        public CodeExecutionHandler(UIApplication app, Document doc, int maxRetries, string prompt)
        {
            Application = app;
            Doc = doc;
            MaxRetries = maxRetries;
            Prompt = prompt;
        }

        public void Execute()
        {
            int retryCount = 0;
            Assembly assembly = Assembly.GetExecutingAssembly();
            string executingAssemblyPath = Path.GetDirectoryName(assembly.Location);
            string revitPythonWrapperPath = executingAssemblyPath + "\\\\Utils";

            if (!PythonEngine.IsInitialized)
            {
                string pythonHome = @"C:\\Users\\franmaranchello\\AppData\\Local\\Programs\\Python\\Python312";
                string pythonDll = Path.Combine(pythonHome, "python312.dll");
                Environment.SetEnvironmentVariable("PYTHONHOME", pythonHome, EnvironmentVariableTarget.Process);
                Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDll, EnvironmentVariableTarget.Process);
                Runtime.PythonDLL = pythonDll;
                PythonEngine.Initialize();
            }

            using (Py.GIL())
            {
                using (PyModule scope = Py.CreateScope())
                {
                    dynamic sys = Py.Import("sys");
                    sys.path.append(revitPythonWrapperPath);
                    scope.Set("app", Application);
                    scope.Set("doc", Doc);
                    scope.Set("__revit__", Application);

                    string code = SendPromptToEndpoint(Prompt);

                    if (!string.IsNullOrEmpty(code))
                    {
                        Code = code;
                    }
                    else
                    {
                        TaskDialog.Show("Zoidberg AI", "Failed to retrieve magic from the AI.");
                        return;
                    }

                    do
                    {
                        try
                        {
                            scope.Exec(Code);
                            scope.Dispose();
                            break;
                        }
                        catch (Exception e)
                        {
                            retryCount++;
                            string promptText = $"The user prompt is: {Prompt}. The python code you previously generated failed with the error: {e.Message}. The current code is: \n\n{Code}. \n\nPlease provide an updated version of the code, addressing the error.";
                            Prompt = promptText;
                            string newCode = SendPromptToEndpoint(Prompt);

                            if (!string.IsNullOrEmpty(newCode))
                            {
                                Code = newCode;
                            }
                            else
                            {
                                TaskDialog.Show("Zoidberg AI", "Failed to retrieve magic from the AI.");
                                return;
                            }
                        }
                    } while (retryCount < MaxRetries);
                }
            }
        }

        private string SendPromptToEndpoint(string promptText)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", "YOUR_API_KEY_HERE");

                string baseUrl = "https://us-central1-ai-bim-assistant-poc.cloudfunctions.net/zoidberg-bim-assistant";
                string instruction = $"?instruction={Uri.EscapeDataString(promptText)}";
                string requestUrl = baseUrl + instruction;

                var response = client.GetAsync(requestUrl).Result;
                string jsonResponse = response.Content.ReadAsStringAsync().Result;

                dynamic jsonObject = JsonConvert.DeserializeObject(jsonResponse);
                string answer = jsonObject.answer;

                string code = CleanCodeSnippet(answer);

                return code;
            }
        }

        private string CleanCodeSnippet(string code)
        {
            string[] lines = code.Trim().Split('\n');

            if (lines[0].Trim().StartsWith("```python"))
            {
                lines = lines.Skip(1).ToArray();
            }

            if (lines[lines.Length - 1].Trim() == "```")
            {
                lines = lines.Take(lines.Length - 1).ToArray();
            }

            string cleanSnippet = string.Join("\n", lines);

            cleanSnippet = cleanSnippet.Replace("\\n", "\n");
            cleanSnippet = cleanSnippet.Replace("\\_", "_");
            cleanSnippet = cleanSnippet.Replace("\\*", "*");

            return cleanSnippet;
        }
    }
}
