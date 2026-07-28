using CondensedCablingBasicLogicComponents.Shared;
using LogicWorld.Server.Circuitry;
using SkysCondensedCablingLib.Server;

namespace CondensedCablingBasicLogicComponents.Server;

public abstract class GenericSuperGate : LogicComponent<SuperSizeSliderData>, IHasSuperPegs
{
    public int PreviousData = default;

    public virtual int InputSuperSize(int index) => Data.CurrentValue;
    public virtual int OutputSuperSize(int index) => Data.CurrentValue;

    protected override void SetDataDefaultValues() => Data.Initialize();

    protected override void OnCustomDataUpdated()
    {
        if (PreviousData == Data.CurrentValue)
        {
            return;
        }

        this.EnsureSuperPegsAreCorrect();

        PreviousData = Data.CurrentValue;
    }
}
