using LogicWorld.GameStates;
using LogicWorld.UI;

namespace AttikittySelectionTools.Client.Commands;

public class ClearSelection : Command
{
    public override Command Inverse => new RestoreSelection(MultiSelector.GetCurrentSelection().Clone());

    public ClearSelection()
    {

    }

    public override void Trigger()
    {
        GameStateManager.TransitionBackToBuildingState();
    }

    public override bool Equals(Command other) => other is ClearSelection;

    public override string ToString()
    {
        return "Clear";
    }
}