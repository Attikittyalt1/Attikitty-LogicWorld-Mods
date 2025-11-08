using FancyInput;
using LogicWorld.Building.Overhaul;
using System;

namespace AttikittySelectionTools.Client.BuildingOperations;

public class ClearAlternateSelection : BuildingOperation
{
    public override string IconHexCode => "3ff3"; //change this
    private static SelectionManager Manager => MyClient.SelectionManager;

    public override bool CanOperateOn(ComponentSelection selection)
    {
        if (!Manager.HasSelection()) return false;

        return true;
    }

    public override void BeginOperationOn(ComponentSelection selection)
    {
        Manager.ClearSelection();
    }

    public override InputTrigger OperationStarter => Inputs.Triggers.ClearAlternateSelection;
}