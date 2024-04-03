using Autodesk.Revit.DB;

namespace Common.Utils
{
    public class ViewportHelper
    {
        public static double GetWidth(Viewport viewport)
        {
            if (viewport == null)
            {
                return 0;
            }

            Outline boundingBoxXYZ = viewport.GetBoxOutline();
            return boundingBoxXYZ.MaximumPoint.X - boundingBoxXYZ.MinimumPoint.X;
        }

        public static double GetHeight(Viewport viewport)
        {
            if (viewport == null)
            {
                return 0;
            }

            Outline boundingBoxXYZ = viewport.GetBoxOutline();
            return boundingBoxXYZ.MaximumPoint.Y - boundingBoxXYZ.MinimumPoint.Y;
        }
    }
}
