using Autodesk.Revit.DB;

namespace Common.Utils
{
    public class PlaneHelper
    {
        /// <summary>
        /// Return signed distance from plane to a given point.
        /// </summary>
        public static double SignedDistanceTo(Plane plane, XYZ p)
        {
            XYZ v = p - plane.Origin;
            return plane.Normal.DotProduct(v);
        }
    }
}
