using System.IO;
using System.Reflection;

namespace Common.Constants
{
    public static class UIConstants
    {
        public static string ButtonIconsFolder
        {
            get
            {
                return AddinPath + "\\Images\\";
            }
        }

        public static string AssemblyPath
        {
            get
            {
                return Assembly.GetExecutingAssembly().Location;
            }
        }

        public static string AddinPath
        {
            get
            {
                return Path.GetDirectoryName(AssemblyPath);
            }
        }
    }
}
