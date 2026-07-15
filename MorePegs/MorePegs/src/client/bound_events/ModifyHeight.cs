using LogicAPI.Data.BuildingRequests;
using LogicWorld.Building.Overhaul;
using LogicWorld.BuildingManagement;
using SkysGeneralLib.Client.TypeExtensions;
using System;
using System.Linq;

namespace MorePegs.Client.BoundEvents;

public abstract class ModifyHeight : BuildingOperation
{
    protected abstract int GetNewInputCount(int inputCount);

    public static int MinHeight = 1;
    public static int MaxHeight = 32;

    public override bool CanOperateOn(ComponentSelection selection)
    {
        if (!selection.ComponentsInSelection.Any(address =>
            address.GetClientCode() is BoardPeg component &&
            Math.Clamp(GetNewInputCount(component.InputCount), MinHeight, MaxHeight) != component.InputCount
        ))
        {
            return false;
        }

        return true;
    }

    public override void BeginOperationOn(ComponentSelection selection)
    {
        foreach (var address in selection.ComponentsInSelection)
        {
            if (address.GetClientCode() is BoardPeg component)
            {
                BuildRequestManager.SendBuildRequest(new BuildRequest_ChangeDynamicComponentPegCounts(
                address,
                Math.Clamp(GetNewInputCount(component.InputCount), MinHeight, MaxHeight),
                0
            ));
            }
        }
    }
}