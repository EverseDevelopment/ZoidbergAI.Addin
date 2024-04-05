using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Common.Application;
using Common.UI.Views;
using MaterialDesignThemes.Wpf;

namespace Common.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class RunZoidberg : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                UIApplication uiApp = commandData.Application;
                Document document = commandData.Application.ActiveUIDocument.Document;
                PromptWindow promptWindow = new PromptWindow(uiApp, document);
                promptWindow.ShowDialog();
                return Result.Succeeded;
            }
            catch (Exception e)
            {
                TaskDialog.Show("Zoidberg AI", $"There was an error: {e.Message}");
                return Result.Failed;
            }
            
        }
    }

}
