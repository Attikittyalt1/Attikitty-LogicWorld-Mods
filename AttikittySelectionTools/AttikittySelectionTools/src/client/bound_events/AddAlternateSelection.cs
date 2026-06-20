using AttikittySelectionTools.Client.Inputs;
using FancyInput;
using LogicWorld.Building.Overhaul;
using LogicWorld.UI;

namespace AttikittySelectionTools.Client.BoundEvents;

public class AddAlternateSelection : BuildingOperation
{
    private static SelectionWorkbench Clipboard => MyClient.SelectionClipboard;

    public override InputTrigger OperationStarter => Triggers.AddAlternateSelection;

    public override bool CanOperateOn(ComponentSelection selection)
    {
        if (selection == null || selection.Count == 0)
        {
            return false;
        }

        if (Clipboard.ContainsEveryAddressFromSelection(selection))
        {
            return false;
        }

        return true;
    }

    public override void BeginOperationOn(ComponentSelection selection)
    {
        Clipboard.AddSelection(selection);
    }

    public static bool TryOperation()
    {
        var selection = MultiSelector.GetCurrentSelection();

        if (selection == null || selection.Count == 0)
        {
            return false;
        }

        if (Clipboard.ContainsEveryAddressFromSelection(selection))
        {
            return false;
        }

        Clipboard.AddSelection(selection);

        return true;
    }
}