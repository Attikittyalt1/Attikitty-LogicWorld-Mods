using LogicAPI.Data;
using LogicWorld.Outlines;
using LogicWorld.UI;
using System;

namespace AttikittySelectionTools.Client.Commands;

public class RemoveComponentFromSelection : Command
{
    private readonly ComponentAddress _address;

    public override Command Inverse => new AddComponentToSelection(_address);

    public RemoveComponentFromSelection(ComponentAddress Address)
    {
        _address = Address;
    }

    public override void Trigger()
    {
        if (!SelectionUtil.CanSelect(_address))
        {
            return;
        }

        var selection = MultiSelector.GetCurrentSelection();

        if (selection == null)
        {
            throw new InvalidOperationException("Tried to deselect component when nothing is selected.");
        }

        if (selection.Contains(_address)) {
            selection.Remove(_address); // this should probably be a reverse patch of RemoveFromSelectionAndRemoveOutline
            Outliner.RemoveHardOutline(_address);
        }
    }

    public override bool Equals(Command other) => other is RemoveComponentFromSelection command && command._address == _address;

    public override string ToString()
    {
        return "Remove " + _address.ToString();
    }
}