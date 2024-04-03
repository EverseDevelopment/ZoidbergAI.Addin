using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;

namespace Common.Utils
{
    public class ElementHelper
    {
        /// <summary>
        /// Get the curves of the element
        /// </summary>
        public static List<Curve> GetCurves(Element elem, Document doc)
        {
            GeometryElement geom = elem.get_Geometry(new Options() { View = doc.ActiveView });
            if (geom == null)
            {
                return new List<Curve>();
            }

            List<Curve> result = new List<Curve>();
            foreach (GeometryObject geomObj in geom)
            {
                if (geomObj is GeometryInstance)
                {
                    GeometryElement instanceGeometry = (geomObj as GeometryInstance).GetInstanceGeometry();
                    foreach (GeometryObject geomObj2 in instanceGeometry)
                    {
                        result.AddRange(GetElementsToInsert(geomObj2));
                    }
                }
                else
                {
                    result.AddRange(GetElementsToInsert(geomObj));
                }
            }

            return result;
        }

        public static bool IsStraight(Element element)
        {
            if (element is FabricationPart)
            {
                FabricationPart part = element as FabricationPart;
                return part.IsAStraight();
            }

            if (element.Location is LocationCurve)
            {
                return true;
            }

            return element is Pipe || element is Duct;
        }

        public static bool IsInvalid(ElementId id)
        {
#if REVIT2024
            return id == null || id.Value == -1;
#else
            return id == null || id.IntegerValue == -1;
#endif

        }

        /// <summary>
        /// Get bounds for an elements in the x-y plane
        /// </summary>
        public static List<Line> GetBounds2D(Element elem, View view)
        {
            BoundingBoxXYZ bounding = elem.get_BoundingBox(view);

            XYZ min = XyzHelper.Flatten(bounding.Min);
            XYZ max = XyzHelper.Flatten(bounding.Max);

            Line line0 = Line.CreateBound(min, new XYZ(min.X, max.Y, 0));
            Line line1 = Line.CreateBound(new XYZ(min.X, max.Y, 0), max);
            Line line2 = Line.CreateBound(max, new XYZ(max.X, min.Y, 0));
            Line line3 = Line.CreateBound(new XYZ(max.X, min.Y, 0), min);
            return new List<Line>() { line0, line1, line2, line3 };
        }

        private static List<Curve> GetElementsToInsert(GeometryObject geomObj2)
        {
            List<Curve> result = new List<Curve>();
            if (geomObj2 is Solid)
            {
                result.AddRange(SolidHelper.GetCurves(geomObj2 as Solid));
            }
            else if (geomObj2 is Curve)
            {
                result.Add(geomObj2 as Curve);
            }
            else if (geomObj2 is Mesh)
            {
                result.AddRange(MeshHelper.ToLines(geomObj2 as Mesh));
            }

            return result;
        }

        public static ConnectorManager GetConnectorManager(Element element)
        {
            ConnectorManager connectorManager;
            switch (element)
            {
                case FabricationPart fabricationPart:
                    connectorManager = fabricationPart.ConnectorManager;
                    break;

                case Conduit conduit:
                    connectorManager = conduit.ConnectorManager;
                    break;

                case CableTray cableTray:
                    connectorManager = cableTray.ConnectorManager;
                    break;

                case Pipe pipe:
                    connectorManager = pipe.ConnectorManager;
                    break;

                case Duct duct:
                    connectorManager = duct.ConnectorManager;
                    break;

                case FamilyInstance familyInstance:
                    connectorManager = familyInstance.MEPModel?.ConnectorManager;
                    break;

                default:
                    connectorManager = null;
                    break;
            }

            return connectorManager;
        }
    }
}
