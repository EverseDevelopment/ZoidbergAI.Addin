using System;

namespace Common.Application
{
    public static class AppConstants
    {
        public const string AddInTabName = "e-verse";  // configure your tab name here!
        public const string AddInToolName = "";
        public const string AddInEnvironment = "development";
        public const string AddInPackageProperty = "";
        public const string AddInAppVersion = "";
        public const string AddInWindowsTitle = "";

        public const string SoftwareName = "Autodesk Revit";

        public static string LogsFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\.ZoidbergAI.Addin";

        private static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }
}

