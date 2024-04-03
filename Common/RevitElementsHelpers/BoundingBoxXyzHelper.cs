using Autodesk.Revit.DB;

namespace Common.Utils
{
    public class BoundingBoxXyzHelper
    {
        public static double GetWidth(BoundingBoxXYZ bounds)
        {
            return bounds.Max.X - bounds.Min.X;
        }

        public static double GetDepth(BoundingBoxXYZ bounds)
        {
            return bounds.Max.Y - bounds.Min.Y;
        }

        public static double GetHeight(BoundingBoxXYZ bounds)
        {
            return bounds.Max.Z - bounds.Min.Z;
        }

        public static XYZ GetCenteroid(BoundingBoxXYZ bounds)
        {
            XYZ vec = bounds.Max - bounds.Min;
            return bounds.Min + (0.5 * vec);
        }

        public static Outline ConvertToOutLine(BoundingBoxXYZ bounds)
        {
            return new Outline(bounds.Min, bounds.Max);
        }
    }
}
