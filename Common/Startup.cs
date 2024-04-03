using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Common.Application;
using Common.UI;

namespace Common
{
    public class Startup : IExternalApplication
    {
        internal Document Document { get; set; }
        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                CreateAddInTab(application);
                ExecuteInjectors(application);

                application.ViewActivated += Application_ViewActivated;

                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show($"OnStartup Error!", ex.ToString());
                return Autodesk.Revit.UI.Result.Failed;
            }
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        private void ExecuteInjectors(UIControlledApplication application)
        {
            // -- Gets types with the capacity of 'Inject'
            Assembly assembly = Assembly.GetExecutingAssembly();
            List<Type> injectorsTypes = assembly.GetTypes().Where(x => typeof(IRibbonInjector).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();

            // -- Get concretes injectors from types.
            List<IRibbonInjector> allInjectors = new List<IRibbonInjector>();
            foreach (Type injector in injectorsTypes)
            {
                // -- Create instance and Inject.
                IRibbonInjector concreteInstance = Activator.CreateInstance(injector.UnderlyingSystemType) as IRibbonInjector;
                allInjectors.Add(concreteInstance);
            }

            // -- Apply order for panels and inject each.
            allInjectors.OrderBy(x => x.Order).ToList().ForEach(i => i.Inject(application));
        }

        private void Application_ViewActivated(object sender, Autodesk.Revit.UI.Events.ViewActivatedEventArgs e)
        {
            Document = e.Document;
        }

        private void CreateAddInTab(UIControlledApplication application)
        {
            // Create the tab
            CreateRibbonTab.Run(application);

            #region Example code to create buttons and panels, you can use it as reference an remove it!
            //PushButtonData pbd = new PushButtonData("Sample", "Click Me", executingAssemblyPath, "SampleRevitAddin.Common.SampleRevitPopup");
            //RibbonPanel panel = application.CreateRibbonPanel(eTabName, "Revit Snack");
            //// Create the main button.
            //PushButton pb = panel.AddItem(pbd) as PushButton;
            //pb.ToolTip = "This is a sample Revit button";
            //pb.LargeImage = ResourceImage.GetIcon("e-verselogo.png");
            #endregion
        }
    }
}
