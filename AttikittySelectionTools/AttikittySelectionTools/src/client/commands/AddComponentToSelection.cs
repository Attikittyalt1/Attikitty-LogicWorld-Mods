using LogicAPI.Data;
using LogicWorld.Building.Overhaul;
using LogicWorld.Outlines;
using LogicWorld.UI;

namespace AttikittySelectionTools.Client.Commands;

public class AddComponentToSelection : Command
{
    private readonly ComponentAddress _address;

    public override Command Inverse => new RemoveComponentFromSelection(_address);

    public AddComponentToSelection(ComponentAddress Address)
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
            MultiSelector.StartWithSelection(new ComponentSelection(_address));
        } else if (!selection.Contains(_address))
        {
            selection.Add(_address); // this should probably be a reverse patch of AddToSelectionAndOutline
            Outliner.HardOutline(_address, OutlineData.Select);
        }
    }

    public override bool Equals(Command other) => other is AddComponentToSelection command && command._address == _address;

    public override string ToString()
    {
        return "Add " + _address.ToString();
    }
}
