using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Common.Utils
{
    public class LineHelper
    {
        public static bool IsPerbindicularOnPlane(Line line, Plane plane)
        {
            XYZ vec = XyzHelper.GetVector(line).Normalize();
            XYZ normal = plane.Normal.Normalize();
            return vec.IsAlmostEqualTo(normal) || vec.IsAlmostEqualTo(-normal);
        }

        public static bool IsLineOnSameExtension2D(Line l1, Line l2, Plane plane)
        {
            Line l1_2D = GetProjection(l1, plane);
            Line l2_2D = GetProjection(l2,  plane);
            if (!IsParallel2D(l1_2D, l2_2D))
            {
                return false;
            }
            
            double dist = XyzHelper.DistanceToProjection(CurveHelper.Start(l1_2D), l2_2D);
            DoubleEqualityComparer comparer = new DoubleEqualityComparer(0.03);
            return comparer.Equals(dist, 0);
        }

        public static bool IsParallel2D(Line l1, Line l2)
        {
            XYZ vec1 = GetVector2D(l1).Normalize();
            XYZ vec2 = GetVector2D(l2).Normalize();
            return vec1.IsAlmostEqualTo(vec2) || vec1.IsAlmostEqualTo(-vec2);
        }

        public static XYZ GetVector2D(Line line)
        {
            var end = CurveHelper.End(line);
            var start = CurveHelper.Start(line);

            return XyzHelper.Flatten(end) - XyzHelper.Flatten(start);
        }

        public static Line GetAdjustedLine(Line line)
        {
            DoubleEqualityComparer comparer = new DoubleEqualityComparer(0.03);
            XYZ start = CurveHelper.Start(line);
            XYZ end = CurveHelper.End(line);

            if (start.X > end.X)
            {
                return Line.CreateBound(end, start);
            }
            else if (comparer.Equals(start.X, end.X))
            {
                if (start.Y > end.Y)
                {
                    return Line.CreateBound(end, start);
                }
                else
                {
                    return line;
                }
            }
            else
            {
                return line;
            }
        }

        public static Line GetProjection(Line line, Plane plane)
        {
            var ep1 = XyzHelper.GetProjection(CurveHelper.Start(line), plane);
            var ep2 = XyzHelper.GetProjection(CurveHelper.End(line), plane);
            return Line.CreateBound(ep1, ep2);
        }
    }

    public class DoubleEqualityComparer : IEqualityComparer<double>
    {
        public double Tol { get; set; } = 0.0000000001;

        public DoubleEqualityComparer()
        {
        }

        public DoubleEqualityComparer(double tol)
        {
            this.Tol = tol;
        }

        public bool Equals(double number1, double number2)
        {
            if (Math.Abs(Math.Abs(number1) - Math.Abs(number2)) < Tol)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(double obj)
        {
            return 0;
        }
    }
}
