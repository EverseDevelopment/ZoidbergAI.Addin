using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Utils
{
    /// <summary>
    /// Helper class for Parameters
    /// </summary>
    public static class ParameterHelper
    {
        /// <summary>
        /// Verify if the element contains BuiltInParameter.
        /// </summary>
        /// <param name="element">Revit Element.</param>
        /// <param name="concreteParameter">Concrete BuiltInParameter to Verify.</param>
        /// <returns>True if Exists.</returns>
        public static bool VerifyIfElementContainsBuiltInParameter(Element element, BuiltInParameter concreteParameter)
        {
            if (element != null && element.Parameters != null && element.Parameters.Size > 0)
            {
                return (from Parameter p in element.Parameters select p)
                    .Any(x => (x.Definition as InternalDefinition)?.BuiltInParameter == concreteParameter);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get all parameters values
        /// </summary>
        /// <param name="element">extension</param>
        /// <param name="paramIds">IEnumerable of ElementId</param>
        public static IReadOnlyDictionary<Autodesk.Revit.DB.ElementId, object> GetAllParameterValues(this Autodesk.Revit.DB.Element element, IEnumerable<Autodesk.Revit.DB.ElementId> paramIds)
        {
            if (element != null)
            {
                MethodInfo method = typeof(Autodesk.Revit.DB.Element).GetMethod("GetAllParameterValues", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[1] { typeof(IEnumerable<Autodesk.Revit.DB.ElementId>) }, null);
                if (method != null)
                {
                    return method.Invoke(element, new object[1] { paramIds }) as IReadOnlyDictionary<Autodesk.Revit.DB.ElementId, object>;
                }
            }
            return null;
        }
    }
}
