using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Common.Application;
using Common.Utils;

namespace Common.Shared
{
    /// <summary>
    /// Shared and common class for Revit Addins.
    /// </summary>
    public abstract class RevitCore
    {
        protected Document Document { get; set; } = ApplicationContext.Instance.GetDocument();
        protected UIDocument UIDocument { get; set; } = ApplicationContext.Instance.GetUIDocument();

        /// <summary>
        /// Filter the Document by class
        /// </summary>
        protected IEnumerable<T> FilterByClass<T>(View activeView = null)
        where T : class
        {
            if (activeView == null)
            {
                using (var collector = new FilteredElementCollector(Document))
                {
                    return collector.OfClass(typeof(T)).Cast<T>();
                }
            }
            else
            {
                using (var collector = new FilteredElementCollector(Document, activeView.Id))
                {
                    return collector.OfClass(typeof(T)).Cast<T>();
                }
            }
        }

        /// <summary>
        /// Filter the document by Category
        /// </summary>
        protected IEnumerable<T> FilterByCategory<T>(BuiltInCategory category)
        where T : class
        {
            using (var collector = new FilteredElementCollector(Document))
            {
                return collector.OfCategory(category)
                        .OfClass(typeof(T))
                        .Cast<T>();
            }
        }

        protected void LogMsg(string msg)
        {
            AppLogger.Instance.AddInformation(msg);
        }

        protected void LogException(Exception ex, string msg)
        {
            AppLogger.Instance.AddEx(ex, msg);
        }
    }
}
