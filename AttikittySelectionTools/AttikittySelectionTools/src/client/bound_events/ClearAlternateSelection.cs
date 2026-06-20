using AttikittySelectionTools.Client.Inputs;
using FancyInput;
using LogicWorld.Building.Overhaul;

namespace AttikittySelectionTools.Client.BoundEvents;

public class ClearAlternateSelection : BuildingOperation
{
    private static SelectionWorkbench Clipboard => MyClient.SelectionClipboard;

    public override InputTrigger OperationStarter => Triggers.ClearAlternateSelection;

    public override bool CanOperateOn(ComponentSelection selection)
    {
        if (!Clipboard.HasSelection())
        {
            return false;
        }

        return true;
    }

    public override void BeginOperationOn(ComponentSelection selection)
    {
        Clipboard.ClearSelection();
    }

    public static bool TryOperation()
    {

        if (!Clipboard.HasSelection())
        {
            return false;
        }

        Clipboard.ClearSelection();

        return true;
    }
}