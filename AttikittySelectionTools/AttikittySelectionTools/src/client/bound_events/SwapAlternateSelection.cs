using AttikittySelectionTools.Client.Commands;
using AttikittySelectionTools.Client.Inputs;
using FancyInput;
using LogicWorld.Building.Overhaul;
using LogicWorld.GameStates;
using LogicWorld.UI;

namespace AttikittySelectionTools.Client.BoundEvents;

public class SwapAlternateSelection : BuildingOperation
{
    private static SelectionWorkbench Clipboard => MyClient.SelectionClipboard;

    public override InputTrigger OperationStarter => Triggers.SwapAlternateSelection;

    public override bool CanOperateOn(ComponentSelection selection)
    {
        var newSelection = Clipboard.GetSelection();

        if (Clipboard.HasExactSelection(selection))
        {
            return false;
        }

        return true;
    }

    public override void BeginOperationOn(ComponentSelection selection)
    {
        var newSelection = Clipboard.GetSelection();

        bool wasEmpty = selection == null;

        Clipboard.SetSelection(selection);

        GameStateManager.TransitionBackToBuildingState();

        if (newSelection != null && SelectionUtil.CanSelectAny(newSelection))
        {
            if (!wasEmpty)
            {
                MyClient.SelectionHistory.StartIgnoringIncomingCommands();
            }

            MultiSelector.StartWithSelection(SelectionUtil.ValidateSelection(newSelection));
            
            if (!wasEmpty)
            {
                MyClient.SelectionHistory.StopIgnoringIncomingCommands();
            }
        }
    }

    public static bool TryOperation()
    {
        var selection = MultiSelector.GetCurrentSelection();
        var newSelection = Clipboard.GetSelection();

        if (Clipboard.HasExactSelection(selection))
        {
            return false;
        }

        bool wasEmpty = selection == null;

        Clipboard.SetSelection(selection);

        GameStateManager.TransitionBackToBuildingState();

        if (newSelection != null && SelectionUtil.CanSelectAny(newSelection)) {
            if (!wasEmpty)
            {
                MyClient.SelectionHistory.StartIgnoringIncomingCommands();
            }

            MultiSelector.StartWithSelection(SelectionUtil.ValidateSelection(newSelection));

            if (!wasEmpty)
            {
                MyClient.SelectionHistory.StopIgnoringIncomingCommands();
            }
        }

        return true;
    }
}