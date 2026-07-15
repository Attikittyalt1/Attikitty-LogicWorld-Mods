using FancyInput;
using JimmysUnityUtilities;
using LICC;
using LogicWorld.Building.Overhaul;
using LogicWorld.Physics;
using LogicWorld.Players;
using MorePegs.Client.Inputs;
using SkysGeneralLib.Client.TypeExtensions;
using System;
using System.Linq;

namespace MorePegs.Client.BoundEvents;

public class ToggleAxis : BuildingOperation
{
    public override InputTrigger OperationStarter => Triggers.ToggleAxis;

    public override bool CanOperateOn(ComponentSelection selection)
    {
        if (selection.ComponentsInSelection.Any(address => address.GetClientCode() is not BoardPeg))
        {
            return false;
        }

        var hit = PlayerCaster.CameraCast(Masks.Peg);

        if (!hit.HitComponent || hit.cAddress.GetClientCode() is not BoardPeg)
        {
            return false;
        }

        return true;
    }

    public override void BeginOperationOn(ComponentSelection selection)
    {
        var hit = PlayerCaster.CameraCast(Masks.Peg);
        var hitComponent = hit.cAddress.GetComponent();
        var point = hitComponent.WorldRotation.Inverse() * hit.RelativePoint;

        bool toggleX = Math.Abs(point.x) > Math.Abs(point.z);
        bool toggleZ = Math.Abs(point.x) < Math.Abs(point.z);

        foreach (var address in selection.ComponentsInSelection)
        {
            if (address.GetClientCode() is BoardPeg component)
            {
                component.Data.ConnectedAxisX ^= toggleX;
                component.Data.ConnectedAxisZ ^= toggleZ;
            }
        }
    }
}