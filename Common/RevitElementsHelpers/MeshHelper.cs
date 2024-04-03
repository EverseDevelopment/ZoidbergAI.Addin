using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace Common.Utils
{
    public class MeshHelper
    {
        public static List<Line> ToLines(Mesh mesh)
        {
            List<Line> result = new List<Line>();
            List<XYZ> vertices = mesh.Vertices.ToList();
            for (int i = 0; i < vertices.Count; i++)
            {
                XYZ pt1 = vertices[i];
                XYZ pt2 = i == vertices.Count - 1 ? vertices[0] : vertices[i + 1];
                if (pt1.DistanceTo(pt2) > 0.003)
                {
                    result.Add(Line.CreateBound(pt1, pt2));
                }
            }

            return result;
        }
    }
}
