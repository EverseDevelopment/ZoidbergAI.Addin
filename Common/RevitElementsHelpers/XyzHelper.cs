using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace Common.Utils
{
    public class XyzHelper
    {
        public static XYZ GetCentroid(Element element)
        {
            XYZ centroid = null;

            BoundingBoxXYZ bb = element.get_BoundingBox(null);
            Line line = Line.CreateBound(bb.Min, bb.Max);
            centroid = line.Evaluate(0.5, true);

            return centroid;
        }

        /// <summary>
        /// Project given 3D XYZ point onto plane.
        /// https://thebuildingcoder.typepad.com/blog/2014/09/planes-projections-and-picking-points.html#12
        /// </summary>
        public static XYZ GetProjection(XYZ p, Plane plane)
        {
            double d = PlaneHelper.SignedDistanceTo(plane, p);
            XYZ q = p - (d * plane.Normal);
            return q;
        }

        /// <summary>
        /// Get projection from point to line in 2D
        /// http://www.sunshine2k.de/coding/java/PointOnLine/PointOnLine.html .
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static XYZ GetProjection(XYZ p, XYZ p1, XYZ p2)
        {
            XYZ e1 = p - p1;
            XYZ e2 = p2 - p1;

            // get dot product of e1, e2
            double len2 = (e2.X * e2.X) + (e2.Y * e2.Y) + (e2.Z * e2.Z);
            double valDp = e1.DotProduct(e2);
            double dist = valDp / len2;
            XYZ proj = new XYZ(p1.X + (e2.X * dist), p1.Y + (e2.Y * dist), p1.Z + (e2.Z * dist));
            return proj;
        }

        public static XYZ GetProjection(XYZ p, Line line)
        {
            return GetProjection(p, CurveHelper.Start(line), CurveHelper.End(line));
        }

        public static List<Line> GetRectangleAround(XYZ pt, View view, double width, double height)
        {
            XYZ right = view.RightDirection.Normalize();
            XYZ up = view.UpDirection.Normalize();
            XYZ topLeft = pt - (right * 0.5 * width) + (up * 0.5 * height);
            XYZ topRight = topLeft + (width * right);
            XYZ bottomRight = topRight - (height * up);
            XYZ bottomLeft = bottomRight - (width * right);
            return new List<Line>()
            {
                Line.CreateBound(topLeft, topRight),
                Line.CreateBound(topRight, bottomRight),
                Line.CreateBound(bottomRight, bottomLeft),
                Line.CreateBound(bottomLeft, topLeft),
            };
        }

        public static XYZ GetVector(Line line)
        {
            return CurveHelper.End(line) - CurveHelper.Start(line);
        }

        public static XYZ Flatten(XYZ point, double height = 0)
        {
            return new XYZ(point.X, point.Y, height);
        }

        public static double DistanceToProjection(XYZ pt, Line line)
        {
            var source = GetProjection(pt, line);
            double dist = pt.DistanceTo(source);
            return dist;
        }

        public static Line GetLongestLineBetweenPoints(List<XYZ> points)
        {
            List<XYZ> sorted = points.OrderBy(o => o.X).ThenBy(o => o.Y).ThenBy(o => o.Z).ToList();
            return Line.CreateBound(sorted[0], sorted[sorted.Count - 1]);
        }

        public static XYZ GetPerbindicularVec2D(XYZ vec, Plane plane)
        {
            XYZ normVec = IsParallel(vec, plane.XVec) ? plane.YVec : plane.XVec;
            XYZ pt1 = plane.Origin;
            XYZ pt2 = plane.Origin + vec;
            XYZ pt = plane.Origin + normVec;
            XYZ proj = GetProjection(vec, pt1, pt2);
            return (pt - proj).Normalize();
        }

        public static bool IsParallel(XYZ vec1, XYZ vec2)
        {
            vec1 = vec1.Normalize();
            vec2 = vec2.Normalize();
            return vec1.IsAlmostEqualTo(vec2) || vec1.IsAlmostEqualTo(-vec2);
        }

        public static bool IsVertical(XYZ vector)
        {
            if (vector.Z == 1 || vector.Z == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsLateral(XYZ vector)
        {
            if (vector.Y == 1 || vector.Y == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
