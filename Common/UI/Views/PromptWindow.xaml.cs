using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System.Windows;
using Common.Commands;

namespace Common.UI.Views
{
    /// <summary>
    /// Interaction logic for PromptWindow.xaml
    /// </summary>
    public partial class PromptWindow : Window
    {
        private Document RevitDocument { get; set; }
        private UIApplication UIApp { get; set; }
        private string CurrentPrompt { get; set; }

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
            CurrentPrompt = PromptBox.Text;

            ExecutePythonCode(UIApp, RevitDocument);
            
        }

        private void ExecutePythonCode(UIApplication uiApp, Document doc, int maxRetries = 3)
        {
            // Create an ExternalEventHandler instance
            var handler = new CodeExecutionHandler(uiApp, doc, maxRetries, CurrentPrompt);

            handler.Execute();
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
