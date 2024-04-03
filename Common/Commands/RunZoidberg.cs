using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class RunZoidberg : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                TaskDialog.Show("e-verse Sample Addin", "Looks like this worked!");
                return Result.Succeeded;
            }
            catch (Exception e)
            {
                TaskDialog.Show("e-verse Sample Addin", $"Exception found: {e.Message}");
                return Result.Failed;
            }
        }
    }

}
