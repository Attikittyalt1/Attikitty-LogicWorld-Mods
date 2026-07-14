using FancyInput;
using JimmysUnityUtilities;
using LogicAPI.Data;
using LogicAPI.Data.BuildingRequests;
using LogicWorld.Building.Overhaul;
using LogicWorld.BuildingManagement;
using LogicWorld.Interfaces;
using LogicWorld.UI;
using MorePegs.Client.Inputs;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System;

namespace MorePegs.Client.BoundEvents;

public class ToggleAxis : BuildingOperation
{
    public override InputTrigger OperationStarter => Triggers.ToggleAxis;

    public override bool CanOperateOn(ComponentSelection selection)
    {
        ComponentType BoardPegType = Instances.MainWorld.ComponentTypes.GetComponentType("MorePegs.BoardPeg");
        return !selection.ComponentsInSelection.Any(address => Instances.MainWorld.Data.Lookup(address).Data.Type != BoardPegType);
    }

    public override void BeginOperationOn(ComponentSelection selection)
    {
        throw new NotImplementedException();

        foreach (var address in selection.ComponentsInSelection)
        {
            var component = Instances.MainWorld.Data.Lookup(address);

            
        }
    }
}