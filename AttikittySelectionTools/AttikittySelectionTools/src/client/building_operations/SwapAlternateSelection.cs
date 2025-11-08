using FancyInput;
using LogicWorld.Building.Overhaul;
using LogicWorld.GameStates;
using LogicWorld.UI;
using System;

namespace AttikittySelectionTools.Client.BuildingOperations;

public class SwapAlternateSelection : BuildingOperation
{
    public override string IconHexCode => "3ff3"; //change this
    private static SelectionManager Manager => MyClient.SelectionManager;

    public override bool CanOperateOn(ComponentSelection selection)
    {
        if (selection.Count == 0) return false;
        if (!Manager.HasSelection()) return false;

        return true;
    }

    public override void BeginOperationOn(ComponentSelection selection)
    {
        var newSelection = Manager.GetSelection();

        Manager.SetSelection(selection);

        GameStateManager.TransitionBackToBuildingState();
        MultiSelector.StartWithSelection(newSelection);
    }

    public override InputTrigger OperationStarter => Inputs.Triggers.SwapAlternateSelection;
}