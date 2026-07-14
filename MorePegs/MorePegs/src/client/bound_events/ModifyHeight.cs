using FancyInput;
using JimmysUnityUtilities;
using LogicAPI.Data;
using LogicAPI.Data.BuildingRequests;
using LogicWorld.Building.Overhaul;
using LogicWorld.BuildingManagement;
using LogicWorld.Interfaces;
using System.Linq;
using System;

namespace MorePegs.Client.BoundEvents;

public abstract class ModifyHeight : BuildingOperation
{
    protected abstract int GetNewInputCount(int inputCount);

    public static int MinHeight = 1;
    public static int MaxHeight = 32;

    public override bool CanOperateOn(ComponentSelection selection)
    {
        return !selection.ComponentsInSelection.Any(address =>
            Instances.MainWorld.ComponentTypes.GetComponentInfo(Instances.MainWorld.Data.Lookup(address).Data.Type).PrefabGeneratorType != typeof(MultiPegPrefabGenerator)
        );
    }

    public override void BeginOperationOn(ComponentSelection selection)
    {
        foreach (var address in selection.ComponentsInSelection)
        {
            var component = Instances.MainWorld.Data.Lookup(address);

            BuildRequestManager.SendBuildRequest(new BuildRequest_ChangeDynamicComponentPegCounts(
                address,
                Math.Clamp(GetNewInputCount(component.Data.InputCount), MinHeight, MaxHeight),
                0
            ));
        }
    }
}