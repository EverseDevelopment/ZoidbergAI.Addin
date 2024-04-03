using Autodesk.Revit.DB;

namespace Common.Utils
{
    public class CurveHelper
    {
        public static XYZ Start(Curve curve)
        {
            return curve.GetEndPoint(0);
        }

        public static XYZ End(Curve curve)
        {
            return curve.GetEndPoint(1);
        }

        public static XYZ GetMidPoint(Curve curve)
        {
            return curve.Evaluate(0.5, true);
        }

        public static (XYZ start, XYZ mid, XYZ end) GetAllPoints(Curve curve)
        {
            return (Start(curve), GetMidPoint(curve), End(curve));
        }

        /// <summary>
        /// Verify if exists intersection between two curves
        /// </summary>
        /// <param name="curve1">First Curve</param>
        /// <param name="curve2">Second Curve</param>
        /// <returns>True if exists</returns>
        public static bool Intersect(Curve curve1, Curve curve2)
        {
            IntersectionResultArray result = null;
            curve1.Intersect(curve2, out result);
            if (result != null)
            {
                return true;
            }

            return false;
        }
    }
}
