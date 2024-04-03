using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Common.Utils
{
    public static class FamilyHelper
    {
        private const string DefFamilyExtension = ".rfa";
        private const string ProjectFamilyFolder = "\\Families\\";

        public static string GetFamilyPath(string familyName)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            var familiyParent = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path)) + ProjectFamilyFolder;
            return Path.Combine(familiyParent, string.Concat(familyName, DefFamilyExtension));
        }

        /// <summary>
        /// A function to load any family, with a set of certain category specified
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="possibleFamilyBuildInCategories"></param>
        /// <param name="familyName"></param>
        internal static Family TryLoadFamily(Document doc, List<BuiltInCategory> possibleFamilyBuildInCategories, string familyName)
        {
            OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.Title = "Load Family";
            dlg.Filter = "rfa files (*.rfa)|*.rfa";

            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return null;
            }

            Family family = null;
            bool loaded = doc.LoadFamily(dlg.FileName, out family);
            if (!loaded)
            {
                TaskDialog.Show("Warning", "Cannot load family.");
                family = null;
            }

#if REVIT2024
            if (!possibleFamilyBuildInCategories.Contains((BuiltInCategory)family.FamilyCategory.Id.Value))
#else
            if (!possibleFamilyBuildInCategories.Contains((BuiltInCategory)family.FamilyCategory.Id.IntegerValue))
#endif

            {
                TaskDialog.Show("Warning", $"Family selected is not a {familyName} family, please try to select a {familyName} family.");
                family = null;
            }

            return family;
        }
    }
}
