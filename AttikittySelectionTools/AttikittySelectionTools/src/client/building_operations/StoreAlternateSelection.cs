using FancyInput;
using LogicWorld.Building.Overhaul;
using System;

namespace AttikittySelectionTools.Client.BuildingOperations;

public class StoreAlternateSelection : BuildingOperation
{
    public override string IconHexCode => "3ff3"; //change this
    private static SelectionManager Manager => MyClient.SelectionManager;

    public override bool CanOperateOn(ComponentSelection selection)
    {
        if (selection.Count == 0) return false;

        return true;
    }

    public override void BeginOperationOn(ComponentSelection selection)
    {
        Manager.SetSelection(selection);
    }

    public override InputTrigger OperationStarter => Inputs.Triggers.StoreAlternateSelection;
}