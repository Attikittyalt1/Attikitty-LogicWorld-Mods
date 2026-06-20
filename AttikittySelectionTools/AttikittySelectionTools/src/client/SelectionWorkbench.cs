using JimmysUnityUtilities;
using LogicWorld.Building.Overhaul;

namespace AttikittySelectionTools.Client;

public class SelectionWorkbench
{
    private ComponentSelection _selection;

    public bool HasSelection()
    {
        return _selection != null && _selection.Count > 0;
    }

    public ComponentSelection GetSelection()
    {
        if (!HasSelection())
        {
            return null;
        }

        return _selection.Clone();
    }

    public void ClearSelection()
    {
        _selection = null;
    }

    public void SetSelection(ComponentSelection currentSelection)
    {
        _selection = currentSelection?.Clone();
    }

    public void AddSelection(ComponentSelection currentSelection)
    {
        if (!HasSelection())
        {
            _selection = currentSelection.Clone();
            return;
        }

        foreach (var address in currentSelection)
        {
            if (!_selection.Contains(address))
            {
                _selection.Add(address);
            }
        }
    }
    public void RemoveSelection(ComponentSelection currentSelection)
    {
        if (_selection == null)
        {
            return;
        }

        foreach (var address in currentSelection)
        {
            if (_selection.Contains(address))
            {
                _selection.Remove(address);
            }
        }
    }

    public bool ContainsAnyAddressFromSelection(ComponentSelection currentSelection)
    {
        if (_selection == null)
        {
            return false;
        }

        foreach (var address in currentSelection)
        {
            if (_selection.Contains(address))
            {
                return true;
            }
        }

        return false;
    }

    public bool ContainsEveryAddressFromSelection(ComponentSelection currentSelection)
    {
        if (_selection == null)
        {
            return false;
        }

        return _selection.ContainsAll(currentSelection);
    }

    public bool HasExactSelection(ComponentSelection selection)
    {
        return _selection.HasTheSameContentsAs_IgnoringOrder(selection);
    }
}
