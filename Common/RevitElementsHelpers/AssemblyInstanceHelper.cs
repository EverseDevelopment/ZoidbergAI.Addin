using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace Common.Utils
{
    public class AssemblyInstanceHelper
    {
        /// <summary>
        /// Gets main (parent) categories for members in an Assembly.
        /// </summary>
        public static List<BuiltInCategory> GetParentCategories(AssemblyInstance assembly, Document doc)
        {
#if REVIT2024
            List<BuiltInCategory> categories = assembly.GetMemberIds()
                                               .Select(x => doc.GetElement(x).Category)
                                               .Where(y => y.Parent == null)
                                               .Select(z => (BuiltInCategory)z.Id.Value)
                                               .Distinct().ToList();
#else
            List<BuiltInCategory> categories = assembly.GetMemberIds()
                                               .Select(x => doc.GetElement(x).Category)
                                               .Where(y => y.Parent == null)
                                               .Select(z => (BuiltInCategory)z.Id.IntegerValue)
                                               .Distinct().ToList();
#endif
            return categories;
        }

        /// <summary>
        /// Get the Sheets of assembly
        /// </summary>
        public static List<ViewSheet> GetViewSheetsFromAssembly(AssemblyInstance assembly)
        {
            List<ViewSheet> sheetViews = new FilteredElementCollector(assembly.Document)
                                       .OfCategory(BuiltInCategory.OST_Sheets)
                                       .OfClass(typeof(ViewSheet))
                                       .Cast<ViewSheet>()
                                       .Where(s => s.AssociatedAssemblyInstanceId == assembly.Id)
                                       .ToList();
            return sheetViews;
        }
    }
}
