using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Python.Runtime;
using System.Reflection;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;

namespace Common.Commands
{
    internal class CodeExecutionHandler
    {
        private UIApplication _uiApp;
        private Document _doc;
        private string _code;
        private int _maxRetries;
        private string _prompt;

        public CodeExecutionHandler(UIApplication uiApp, Document doc, int maxRetries, string prompt)
        {
            _uiApp = uiApp;
            _doc = doc;
            _maxRetries = maxRetries;
            _prompt = prompt;
        }

        public void Execute()
        {
            int retryCount = 0;
            Assembly assembly = Assembly.GetExecutingAssembly();
            string executingAssemblyPath = Path.GetDirectoryName(assembly.Location);
            string revitPythonWrapperPath = executingAssemblyPath + "\\\\Utils";

            if(!PythonEngine.IsInitialized)
            {
                string pythonHome = @"C:\\Users\\franmaranchello\\AppData\\Local\\Programs\\Python\\Python312";
                string pythonDll = Path.Combine(pythonHome, "python312.dll");
                // Set the Python home and the path to the Python DLL
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
                    scope.Set("app", _uiApp);
                    scope.Set("doc", _doc);
                    scope.Set("__revit__", _uiApp);

                    string code = SendPromptToEndpoint(_prompt);
                    if (!string.IsNullOrEmpty(code))
                    {
                        _code = code;
                    }
                    else
                    {
                        TaskDialog.Show("Zoidberg AI", "Failed to retrieve magic from the AI.");
                    }

                    while (retryCount < _maxRetries)
                    {
                        try
                        {
                            //using (Transaction t = new Transaction(_doc, "Automatic Zoidberg Action"))
                            //{
                            //    t.Start();
                                scope.Exec(_code);
                                //t.Commit();
                                scope.Dispose();
                                break; // If the code executes successfully, exit the loop
                            //}
                        }
                        catch (Exception ex)
                        {
                            retryCount++;

                            if (retryCount < _maxRetries)
                            {
                                // Ask the AI for new code based on the exception
                                string promptText = $"The user prompt is: {_prompt}.The Python code you previously generated failed with the error: {ex.Message}\n\n{_code}\n\nPlease provide an updated version of the code that addresses the error.";
                                string newCode = SendPromptToEndpoint(promptText);

                                if (!string.IsNullOrEmpty(newCode))
                                {
                                    _code = newCode;
                                }
                                else
                                {
                                    TaskDialog.Show("Zoidberg AI", "Failed to retrieve updated magic from the AI.");
                                    break;
                                }
                            }
                            else
                            {
                                TaskDialog.Show("Zoidberg AI", $"There was an error after {_maxRetries} attempts: {ex.Message}");
                                scope.Dispose();
                            }

                        }
                    }
                }
            }
        }

        private string SendPromptToEndpoint(string promptText)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", "AIzaSyCbHYQWlCO8n03T-uJ5VGrZTkxybVX3u2Q");

                string baseUrl = "https://us-central1-ai-bim-assistant-poc.cloudfunctions.net/zoidberg-bim-assistant";
                string instruction = $"?instruction={Uri.EscapeDataString(promptText)}";
                string requestUrl = baseUrl + instruction;

                var response = client.GetAsync(requestUrl).Result;
                string jsonResponse = response.Content.ReadAsStringAsync().Result;

                // Deserialize the JSON response
                dynamic jsonObject = JsonConvert.DeserializeObject(jsonResponse);
                string answer = jsonObject.answer;

                // Clean the code snippet
                string code = CleanCodeSnippet(answer);

                return code;
            }
        }

        private string CleanCodeSnippet(string code)
        {
            // Split the snippet into lines
            string[] lines = code.Trim().Split('\n');

            // Remove the first and last lines if they contain the backticks and language identifier
            if (lines[0].Trim().StartsWith("```python"))
            {
                lines = lines.Skip(1).ToArray();
            }
            if (lines[lines.Length - 1].Trim() == "```")
            {
                lines = lines.Take(lines.Length - 1).ToArray();
            }

            // Join the lines back into a single string
            string cleanSnippet = string.Join("\n", lines);

            // Clean up the code
            cleanSnippet = cleanSnippet.Replace("\\n", "\n");
            cleanSnippet = cleanSnippet.Replace("\\_", "_");
            cleanSnippet = cleanSnippet.Replace("\\*", "*");

            return cleanSnippet;
        }
    }
}