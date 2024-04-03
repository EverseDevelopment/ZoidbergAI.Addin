using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Common.Utils
{
    public class SolidHelper
    {
        public static List<Curve> GetCurves(Solid solid)
        {
            List<Curve> result = new List<Curve>();
            foreach (Face face in solid.Faces)
            {
                result.AddRange(FaceHelper.GetCurves(face));
            }

            foreach (Edge item in solid.Edges)
            {
                result.Add(item.AsCurve());
            }

            return result;
        }
    }
}
