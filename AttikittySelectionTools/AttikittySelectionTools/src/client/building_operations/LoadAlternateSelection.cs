using FancyInput;
using LogicWorld.Building.Overhaul;
using LogicWorld.GameStates;
using LogicWorld.UI;
using System;

namespace AttikittySelectionTools.Client.BuildingOperations;

public class LoadAlternateSelection : BuildingOperation
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
        var newSelection = Manager.GetSelection();

        GameStateManager.TransitionBackToBuildingState();
        MultiSelector.StartWithSelection(newSelection);
    }

    public override InputTrigger OperationStarter => Inputs.Triggers.LoadAlternateSelection;
}