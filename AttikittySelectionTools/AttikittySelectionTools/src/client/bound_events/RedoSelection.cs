using AttikittySelectionTools.Client.Inputs;
using FancyInput;
using LogicWorld.Building.Overhaul;

namespace AttikittySelectionTools.Client.BoundEvents;

public class RedoSelection : BuildingOperation
{
    private static CommandManager History => MyClient.SelectionHistory;

    public override InputTrigger OperationStarter => Triggers.RedoSelection;

    public override bool CanOperateOn(ComponentSelection selection)
    {
        if (!History.CanRedo())
        {
            return false;
        }

        return true;
    }

    public override void BeginOperationOn(ComponentSelection selection)
    {
        History.Redo();
    }

    public static bool TryOperation()
    {
        if (!History.CanRedo())
        {
            return false;
        }

        History.Redo();

        return true;
    }
}