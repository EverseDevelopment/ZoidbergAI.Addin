using System;
using System.IO;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;
using Common.Constants;

namespace Common.UI.Base
{
    public abstract class PushButtonBase
    {
        public abstract string Name { get; }

        public abstract string Text { get; }

        public abstract string ClassName { get; }

        public abstract string LargeImage { get; }

        public abstract string Image { get; }

        public abstract string ToolTip { get; }

        public abstract string LongDescription { get; }

        public abstract string HelpPath { get; }

        public PushButtonData GetPushButtonData()
        {
            PushButtonData pushButton = new PushButtonData(this.Name,
                this.Text, UIConstants.AssemblyPath, this.ClassName);

            pushButton.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                this.LargeImage), UriKind.Absolute));

            pushButton.Image =
                new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                this.Image), UriKind.Absolute));

            pushButton.ToolTip = this.ToolTip;
            pushButton.LongDescription = this.LongDescription;

            //pushButton.SetContextualHelp(new
            //    ContextualHelp(ContextualHelpType.Url, Common.Documentation.DocumentationHelper.BuildUri(this.HelpPath)));

            return pushButton;
        }
    }
}
