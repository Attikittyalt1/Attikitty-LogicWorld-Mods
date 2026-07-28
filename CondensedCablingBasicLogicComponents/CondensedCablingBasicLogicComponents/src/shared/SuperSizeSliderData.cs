namespace CondensedCablingBasicLogicComponents.Shared;

public interface SuperSizeSliderData : SingleSliderData
{

    int SingleSliderData.GetDefault() => 1;
    int SingleSliderData.GetMax() => 256;
    int SingleSliderData.GetMin() => 1;
    int SingleSliderData.GetInterval() => 1;
}