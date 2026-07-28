namespace CondensedCablingBasicLogicComponents.Shared;

public interface SingleSliderData
{
    int CurrentValue { get; set; }

    int GetDefault();
    int GetMax();
    int GetMin();
    int GetInterval();

    void Initialize()
    {
        CurrentValue = GetDefault();
    }
}