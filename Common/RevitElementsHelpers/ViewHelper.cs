using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Common.Utils
{
    public class ViewHelper
    {
        /// <summary>
        /// Returns the 3DView of the View
        /// </summary>
        /// <param name="document">Document</param>
        /// <param name="viewName">Name of the View</param>
        /// <returns>3DView or null if not exists</returns>
        public static View3D Get3DView(Document document, string viewName)
        {
            View3D default3DView = new FilteredElementCollector(document)
                .OfClass(typeof(View3D))
                .Cast<View3D>().Where(x => !x.IsTemplate)
                .FirstOrDefault(x => x.Name.Contains(viewName));

            return default3DView;
        }

        /// <summary>
        /// Creates a new 3D View
        /// </summary>
        /// <param name="document">Document</param>
        /// <param name="viewName">Name of the View</param>
        /// <returns>A new 3DView</returns>
        public static View3D CreateNew3DView(Document document, string viewName)
        {
            ViewFamilyType viewFamilyType = (from v in new FilteredElementCollector(document).OfClass(typeof(ViewFamilyType)).Cast<ViewFamilyType>()
                                             where v.ViewFamily == ViewFamily.ThreeDimensional
                                             select v).FirstOrDefault();
            View new3DView = null;

            using (Transaction t = new Transaction(document, "Create3DView"))
            {
                new3DView = View3D.CreateIsometric(document, viewFamilyType.Id);
                new3DView.Name = viewName;
            }
            
            return new3DView as View3D;
        }

        /// <summary>
        /// Lock the 3D View
        /// </summary>
        /// <param name="view3D">View3D</param>
        /// <param name="doc">Document</param>
        public static void Lock3DView(Document doc, View3D view3D)
        {
            if (!view3D.IsLocked)
            {
                using (Transaction t = new Transaction(doc, "SaveOrientationAndLock"))
                {
                    view3D.SaveOrientationAndLock();
                }
            }
        }

        public static bool OpenViews(Document doc, UIDocument uidoc, IList<ElementId> viewsIds, OpenViewType? enumFilter = null)
        {
            IList<View> views;
            try
            {
                views = viewsIds.Select(x => doc.GetElement(x) as View).Where(x => x != null).ToList();
            }
            catch (Exception)
            {
                TaskDialog.Show("Error", "Please select the views you want to open from the Project Browser"); // -- ToDo config from outside
                return false;
            }

            if (!views.Any())
            {
                TaskDialog.Show("Error", "Please select the views you want to open from the Project Browser"); // -- ToDo config from outside
                return false;
            }

            foreach (View view in views)
            {
                // Filter by enum
                if (enumFilter != null)
                {
                    if (enumFilter == OpenViewType.SHEETS && !(view is ViewSheet))
                    {
                        continue;
                    }
                    else if (enumFilter == OpenViewType.VIEWS && view is ViewSheet)
                    {
                        continue;
                    }
                }

                uidoc.RequestViewChange(view);
            }

            return true;
        }
    }

    public enum OpenViewType
    {
        VIEWS,
        SHEETS,
        VIEWSANDSHEETS,
    }
}
