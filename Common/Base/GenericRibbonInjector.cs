using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;
using Common.Application;
using Common.UI.Base;

namespace Common.Base
{
    public abstract class GenericRibbonInjector : IRibbonInjector
    {
        public abstract int Order { get; }

        protected abstract string PanelName { get; }

        public RibbonPanel RibbonPanel { get; set; }

        public virtual void Inject(UIControlledApplication application)
        {
            // Get current panels
            IList<RibbonPanel> ribbonPanels = application.GetRibbonPanels();

            // Check if the panel already exists
            RibbonPanel existingPanel = ribbonPanels.Where(x => x.Name == PanelName).FirstOrDefault();

            if (existingPanel != null)
            {
                RibbonPanel = ribbonPanels.Where(x => x.Name == PanelName).First();
            }
            else
            {
                RibbonPanel = application.CreateRibbonPanel(AppConstants.AddInTabName, PanelName);
            }

            Register();
        }

        public abstract void Register();

        public PushButtonData CreatePushButton<T>(bool addToMainRibbon = true)
        where T : PushButtonBase, new()
        {
            PushButtonData item = new T().GetPushButtonData();
            if (addToMainRibbon)
            {
                this.RibbonPanel.AddItem(item);
            }

            return item;
        }
    }
}
