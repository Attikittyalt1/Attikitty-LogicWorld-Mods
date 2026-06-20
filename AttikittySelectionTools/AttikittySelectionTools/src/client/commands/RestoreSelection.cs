using LogicWorld.Building.Overhaul;
using LogicWorld.GameStates;
using LogicWorld.UI;
using JimmysUnityUtilities;

namespace AttikittySelectionTools.Client.Commands;

public class RestoreSelection : Command
{
    private readonly ComponentSelection _selection;

    public override Command Inverse => new ClearSelection();

    public RestoreSelection(ComponentSelection Selection)
    {
        _selection = Selection;
    }

    public override void Trigger()
    {
        GameStateManager.TransitionBackToBuildingState();

        if (_selection != null && SelectionUtil.CanSelectAny(_selection))
        {
            MultiSelector.StartWithSelection(SelectionUtil.ValidateSelection(_selection));
        }
    }

    public override bool Equals(Command other) => other is RestoreSelection command && command._selection.HasTheSameContentsAs_IgnoringOrder(_selection);

    public override string ToString()
    {
        return "Restore " + _selection.Count;
    }
}