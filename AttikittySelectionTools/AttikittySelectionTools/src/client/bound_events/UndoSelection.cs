using AttikittySelectionTools.Client.Inputs;
using FancyInput;
using LogicWorld.Building.Overhaul;

namespace AttikittySelectionTools.Client.BoundEvents;

public class UndoSelection : BuildingOperation
{
    private static CommandManager History => MyClient.SelectionHistory;

    public override InputTrigger OperationStarter => Triggers.UndoSelection;

    public override bool CanOperateOn(ComponentSelection selection)
    {
        if (!History.CanUndo())
        {
            return false;
        }

        return true;
    }

    public override void BeginOperationOn(ComponentSelection selection)
    {
        History.Undo();
    }

    public static bool TryOperation()
    {
        if (!History.CanUndo())
        {
            return false;
        }

        History.Undo();

        return true;
    }
}