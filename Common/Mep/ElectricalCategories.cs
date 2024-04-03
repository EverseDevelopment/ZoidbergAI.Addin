using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Common.Mep
{
    public static class ElectricalCategories
    {
        public static readonly int OstConduitCategory = (int)BuiltInCategory.OST_Conduit;

        public static readonly int OstConduitFittingCategory = (int)BuiltInCategory.OST_ConduitFitting;

        private static List<int> allCategories;

        public static List<int> AllCategories
        {
            get
            {
                if (allCategories == null)
                {
                    allCategories = new List<int>() { OstConduitCategory, OstConduitFittingCategory };
                }

                return allCategories;
            }
        }
    }
}
