using Autodesk.Revit.DB;

namespace Common.Utils
{
    public class LocationExtension
    {
        /// <summary>
        /// Get Line from Location
        /// </summary>
        public static Line GetLine(Location location)
        {
            if (!(location is LocationCurve))
            {
                return null;
            }

            Curve curve = (location as LocationCurve).Curve;
            return !(curve is Line) ? null : curve as Line;
        }
    }
}
