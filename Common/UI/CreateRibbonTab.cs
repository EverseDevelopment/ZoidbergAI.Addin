using System.Linq;
using Autodesk.Revit.UI;
using Common.Application;

namespace Common.UI
{
    public static class CreateRibbonTab
    {
        /// <summary>
        /// Check if certain tab exists if not create it.
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public static bool Run(UIControlledApplication application)
        {
            bool tabExists = Autodesk.Windows.ComponentManager.Ribbon.Tabs.Any(tab => tab.Name == AppConstants.AddInTabName);
            if (!tabExists)
            {
                application.CreateRibbonTab(AppConstants.AddInTabName);
            }

            return true;
        }
    }
}
