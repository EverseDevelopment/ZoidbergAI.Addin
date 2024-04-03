using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Common.Mep
{
    [Serializable]
    public class MepCategoriesDictionary : Dictionary<BuiltInCategory, BuiltInCategory>
    {
        public MepCategoriesDictionary()
        {
            // ELECTRICAL
            Add(BuiltInCategory.OST_Conduit, BuiltInCategory.OST_ConduitTags);
            Add(BuiltInCategory.OST_ConduitFitting, BuiltInCategory.OST_ConduitFittingTags);
            Add(BuiltInCategory.OST_ElectricalCircuit, BuiltInCategory.OST_ElectricalCircuitTags);
            Add(BuiltInCategory.OST_ElectricalFixtures, BuiltInCategory.OST_ElectricalFixtureTags);
            Add(BuiltInCategory.OST_ElectricalEquipment, BuiltInCategory.OST_ElectricalEquipmentTags);

            // MECHANICAL
            Add(BuiltInCategory.OST_DuctCurves, BuiltInCategory.OST_DuctTags);
            Add(BuiltInCategory.OST_FlexDuctCurves, BuiltInCategory.OST_FlexDuctTags);
            Add(BuiltInCategory.OST_FabricationDuctwork, BuiltInCategory.OST_FabricationDuctworkTags);
            Add(BuiltInCategory.OST_DuctTerminal, BuiltInCategory.OST_DuctTerminalTags);
            Add(BuiltInCategory.OST_DuctAccessory, BuiltInCategory.OST_DuctAccessoryTags);
            Add(BuiltInCategory.OST_DuctFitting, BuiltInCategory.OST_DuctFittingTags);
            Add(BuiltInCategory.OST_MechanicalEquipment, BuiltInCategory.OST_MechanicalEquipmentTags);
            Add(BuiltInCategory.OST_MechanicalEquipmentSet, BuiltInCategory.OST_MechanicalEquipmentSetTags);

            // PLUMBING
            Add(BuiltInCategory.OST_PipeCurves, BuiltInCategory.OST_PipeTags);
            Add(BuiltInCategory.OST_FlexPipeCurves, BuiltInCategory.OST_FlexPipeTags);
            Add(BuiltInCategory.OST_FabricationPipework, BuiltInCategory.OST_FabricationPipeworkTags);
            Add(BuiltInCategory.OST_PipeFitting, BuiltInCategory.OST_PipeFittingTags);
            Add(BuiltInCategory.OST_PipeAccessory, BuiltInCategory.OST_PipeAccessoryTags);
            Add(BuiltInCategory.OST_PlumbingFixtures, BuiltInCategory.OST_PlumbingFixtureTags);

            // FABRICATION PART
            Add(BuiltInCategory.OST_FabricationHangers, BuiltInCategory.OST_FabricationHangerTags);
        }
    }
}
