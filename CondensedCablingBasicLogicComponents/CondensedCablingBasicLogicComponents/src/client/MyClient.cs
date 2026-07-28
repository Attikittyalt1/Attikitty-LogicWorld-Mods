using EccsLogicWorldAPI.Client.Hooks;
using LogicAPI.Client;
using LogicWorld;
using System;

namespace CondensedCablingBasicLogicComponents.Client;

public class MyClient : ClientMod
{
    protected override void Initialize()
    {
        WorldHook.worldLoading += () =>
        {
            //This action is in Unity execution scope, errors must be caught manually:
            try
            {
                SuperSizeSliderGUI.Build(
                    "CondensedCablingBasicLogicComponents.Gui.GenericSuperComponent",
                    "CondensedCablingBasicLogicComponents.Gui.GenericSuperComponent.SuperSize"
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
