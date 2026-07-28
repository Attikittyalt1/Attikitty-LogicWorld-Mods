using CondensedCablingBasicLogicComponents.Shared;
using LogicWorld.Rendering.Components;

namespace CondensedCablingBasicLogicComponents.Client;

public class GenericSuperGate : ComponentClientCode<SuperSizeSliderData>
{
    protected override void SetDataDefaultValues() => Data.Initialize();
}