using AttikittySelectionTools.Client.Inputs;
using FancyInput;
using LogicWorld.Building.Overhaul;

namespace AttikittySelectionTools.Client.BoundEvents;

public class ClearSelectionHistory : BuildingOperation
{
    private static CommandManager History => MyClient.SelectionHistory;

    public override InputTrigger OperationStarter => Triggers.ClearSelectionHistory;

    public override bool CanOperateOn(ComponentSelection selection)
    {
        if (History.IsEmpty())
        {
            return false;
        }

        return true;
    }

    public override void BeginOperationOn(ComponentSelection selection)
    {
        History.Clear();
    }

    public static bool TryOperation()
    {
        if (History.IsEmpty())
        {
            return false;
        }

        History.Clear();

        return true;
    }
}