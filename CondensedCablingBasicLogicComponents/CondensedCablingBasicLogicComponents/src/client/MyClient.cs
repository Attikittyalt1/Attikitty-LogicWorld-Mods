using EccsLogicWorldAPI.Client.Hooks;
using LogicAPI.Client;
using LogicSettings;
using LogicWorld;
using System;

namespace CondensedCablingBasicLogicComponents.Client;

public class MyClient : ClientMod
{
    [Setting_SliderInt("CondensedCablingBasicLogicComponents.MaxSuperSize")]
    public static int MaxSuperSize { get; set; } = 8;

    protected override void Initialize()
    {
        WorldHook.worldLoading += () =>
        {
            //This action is in Unity execution scope, errors must be caught manually:
            try
            {
                EditGenericSingleSliderSuperGate.Build(
                    "CondensedCablingBasicLogicComponents.GenericSingleSliderSuperGateMenu",
                    "SliderContainer",
                    "CondensedCablingBasicLogicComponents.Gui.GenericSingleSliderSuperGate.Slider"
                );
                EditGenericDoubleSliderSuperGate.Build(
                    "CondensedCablingBasicLogicComponents.GenericDoubleSliderSuperGateMenu",
                    "SliderContainerA",
                    "SliderContainerB",
                    "CondensedCablingBasicLogicComponents.Gui.GenericDoubleSliderSuperGate.SliderA",
                    "CondensedCablingBasicLogicComponents.Gui.GenericDoubleSliderSuperGate.SliderB"
                );
            }
            catch (Exception e)
            {
                Logger.Error($"Failed to initialize GUI for {Manifest.Name}:");
                SceneAndNetworkManager.TriggerErrorScreen(e);
            }
        };
    }
}
