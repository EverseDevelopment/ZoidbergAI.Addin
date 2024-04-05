using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Windows;
using Microsoft.Scripting.Hosting;
using System.IO;
using System.Reflection;
using Python.Runtime;

namespace Common.UI.Views
{
    /// <summary>
    /// Interaction logic for PromptWindow.xaml
    /// </summary>
    public partial class PromptWindow : Window
    {
        private Document RevitDocument { get; set; }
        private UIApplication UIApp { get; set; }
        public PromptWindow(UIApplication uiApp, Document doc)
        {
            ColorZoneAssist.SetMode(new System.Windows.Controls.GroupBox(), ColorZoneMode.Dark);
            _ = new Hue("name", System.Windows.Media.Color.FromArgb(1, 2, 3, 4), System.Windows.Media.Color.FromArgb(1, 5, 6, 7));
            UIApp = uiApp;
            RevitDocument = doc;
            InitializeComponent();
        }
        private void SendPrompt_Click(object sender, RoutedEventArgs e)
        {
            string promptText = PromptBox.Text;
            string code = SendPromptToEndpoint(promptText);

            if (!string.IsNullOrEmpty(code))
            {
                ExecutePythonCode(code, UIApp, RevitDocument);
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

        private void ExecutePythonCode(string code, UIApplication uiApp, Document doc)
        {
            try
            {
                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Automatic Zoidberg Action");
                    //                    var engine = Python.CreateEngine();
                    //                    var scope = engine.CreateScope();

                    Assembly assembly = Assembly.GetExecutingAssembly();
                    string executingAssemblyPath = Path.GetDirectoryName(assembly.Location);
                    string revitPythonWrapperPath = executingAssemblyPath + "\\Utils";


                    //                    // Load the rpw package modules into the scope
                    //                    //LoadPackageModules(engine, scope, revitPythonWrapperPath);

                    //                    // Add the parent folder path to the engine's search paths
                    //                    engine.SetSearchPaths(new[] { executingAssemblyPath, revitPythonWrapperPath });
                    //                    engine.Runtime.LoadAssembly(typeof(Document).Assembly);
                    //                    engine.Runtime.LoadAssembly(typeof(UIApplication).Assembly);

                    //                    // Set up the Revit application and document in the scope
                    //                    scope.SetVariable("__revit__", uiApp.Application);
                    //                    scope.SetVariable("__document__", doc);


                    //                    // Initialize rpw in the current context
                    //                    var initScript = @"
                    //import sys
                    //sys.path.append(r'" + revitPythonWrapperPath + @"')
                    //import rpw
                    //from rpw import revit, db, ui, DB, UI
                    //from rpw.utils.logger import logger
                    //";
                    //                    engine.Execute(initScript, scope);

                    //                    // Execute the Python code
                    //                    var script = engine.CreateScriptSourceFromString(code);
                    //                    script.Execute(scope);

                    // Initialize the Python engine and set up the Python environment

                    // Assuming your Python version is 3.8 and it's installed in C:\Python38
                    string pythonHome = @"C:\Users\franmaranchello\AppData\Local\Programs\Python\Python312";
                    string pythonDll = System.IO.Path.Combine(pythonHome, "python312.dll");

                    // Set the Python home and the path to the Python DLL
                    Environment.SetEnvironmentVariable("PYTHONHOME", pythonHome, EnvironmentVariableTarget.Process);
                    Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDll, EnvironmentVariableTarget.Process);

                    Python.Runtime.Runtime.PythonDLL = pythonDll; // Explicitly set the Python DLL

                    PythonEngine.Initialize();
                    using (Py.GIL()) // Acquire the Global Interpreter Lock (GIL)
                    {
                        using (PyModule scope = Py.CreateScope())
                        {
                            dynamic sys = Py.Import("sys");
                            sys.path.append(revitPythonWrapperPath);
                            scope.Set("app", uiApp);
                            scope.Set("doc", doc);
                            // Add any paths to sys.path if necessary, for example:
                            // sys.path.append("path_to_your_python_modules_or_virtualenv");

                            // Execute your Python script here
                            // Assuming 'code' contains the path to the Python script file
                            //dynamic pyScript = Py.Import(System.IO.Path.GetFileNameWithoutExtension(code));

                            // If 'code' is a string containing Python code rather than a file path:
                            scope.Exec(code);

                            // For more direct interaction with Python objects, use:
                            // dynamic result = pyScript.YourFunction(); // Replace 'YourFunction' with an actual function name
                            // Console.WriteLine(result);

                        }
                    }

                    t.Commit();
                }
            }
            catch(Exception ex)
            {
                TaskDialog.Show("Zoidberg AI", $"There was an error: {ex.Message}");
            }
            finally
            {
                PythonEngine.Shutdown();
            }
        }

        //private void LoadPackageModules(ScriptEngine engine, ScriptScope scope, string packagePath)
        //{
        //    foreach (var file in Directory.GetFiles(packagePath, "*.py", SearchOption.AllDirectories))
        //    {
        //        var moduleName = Path.GetFileNameWithoutExtension(file);

        //        engine.ExecuteFile(file, scope);

        //        var module = engine.Runtime.ImportModule(moduleName);
        //        scope.SetVariable(moduleName, module);
        //    }
        //}
    }
}
