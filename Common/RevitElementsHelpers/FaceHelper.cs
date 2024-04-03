using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Common.Utils
{
    public class FaceHelper
    {
        /// <summary>
        /// Get Curves from Face
        /// </summary>
        public static List<Curve> GetCurves(Face face)
        {
            List<Curve> result = new List<Curve>();
            foreach (EdgeArray array in face.EdgeLoops)
            {
                foreach (Edge edge in array)
                {
                    result.Add(edge.AsCurve());
                }
            }

            return result;
        }
    }
}
