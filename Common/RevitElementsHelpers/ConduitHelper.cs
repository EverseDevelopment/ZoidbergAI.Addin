using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Common.Mep;

namespace Common.Utils
{
    public class ConduitHelper
    {
        public static List<ElementId> GetConduitsConnectedElements(Document doc, Element element, List<ElementId> elementIds)
        {
            ConnectorSet connectors = ElementHelper.GetConnectorManager(element).Connectors;

            foreach (Connector connector in connectors)
            {
                foreach (Connector subConnector in connector.AllRefs)
                {
                    if (!elementIds.Contains(subConnector.Owner.Id))
                    {
                        Element connectedElement = doc.GetElement(subConnector.Owner.Id);
#if REVIT2024
                        if (ElectricalCategories.AllCategories.Contains((int)connectedElement.Category.Id.Value))
#else
                        if (ElectricalCategories.AllCategories.Contains(connectedElement.Category.Id.IntegerValue))
#endif
                        {
                            elementIds.Add(connectedElement.Id);
                            elementIds = GetConduitsConnectedElements(doc, connectedElement, elementIds);
                        }
                    }
                }
            }

            return elementIds.Distinct().ToList();
        }
    }
}
