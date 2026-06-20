using LogicAPI.Data;
using LogicWorld.Building.Overhaul;
using LogicWorld.Interfaces;
using System.Linq;

namespace AttikittySelectionTools.Client;

public static class SelectionUtil
{

    public static bool CanSelect(ComponentAddress address)
        => Instances.MainWorld.Data.Lookup(address) != null;

    public static bool CanSelectAll(ComponentSelection selection)
        => selection.ToList().All(CanSelect);

    public static bool CanSelectAny(ComponentSelection selection)
        => selection.ToList().Any(CanSelect);

    public static ComponentSelection ValidateSelection(ComponentSelection selection)
        => [.. selection.ToList().Where(CanSelect)];
}