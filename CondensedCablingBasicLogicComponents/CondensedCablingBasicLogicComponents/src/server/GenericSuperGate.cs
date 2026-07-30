using CondensedCablingBasicLogicComponents.Shared;
using LogicWorld.Server.Circuitry;
using SkysCondensedCablingLib.Server;

namespace CondensedCablingBasicLogicComponents.Server;

public abstract class GenericSuperGate : LogicComponent<SuperSizeSliderData>, IHasSuperPegs
{
    public int PreviousSize = default;

    public virtual int InputSuperSize(int index) => Data.CurrentValue;
    public virtual int OutputSuperSize(int index) => Data.CurrentValue;

    protected override void SetDataDefaultValues() => Data.Initialize();

    protected override void OnCustomDataUpdated()
    {
        if (PreviousSize == Data.CurrentValue)
        {
            return;
        }

        this.EnsureSuperPegsAreCorrect();

        PreviousSize = Data.CurrentValue;
    }
}
