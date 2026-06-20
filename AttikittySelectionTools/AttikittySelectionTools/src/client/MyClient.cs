using AttikittySelectionTools.Client.BoundEvents;
using AttikittySelectionTools.Client.Commands;
using AttikittySelectionTools.Client.Inputs;
using FancyInput;
using HarmonyLib;
using LogicAPI.Client;
using LogicAPI.Data;
using LogicWorld;
using LogicWorld.UI;
using EccsLogicWorldAPI.Client.Hooks;
using LogicSettings;
using LogicWorld.Building.Overhaul;

namespace AttikittySelectionTools.Client;

public class MyClient : ClientMod
{
    public const bool DEBUG = false;

    public static SelectionWorkbench SelectionClipboard;
    public static CommandManager SelectionHistory;

    [Setting_SliderInt("AttikittySelectionTools.MaxHistoryLength")]
    public static int MaxHistoryLengthSetting { get; set; } = 5;

    protected override void Initialize()
    {
        InitializeKeybinds();

        var harmony = new Harmony("AttikittySelectionToolsClient");
        harmony.PatchAll();

        CustomInput.Register<Context, Triggers>("AttikittySelectionTools");

        WorldHook.worldLoading += WorldHook_worldLoading;
        WorldHook.worldUnloading += WorldHook_worldUnloading;
    }

    private void WorldHook_worldLoading()
    {
        SelectionClipboard = new();
        SelectionHistory = new() { MaxCount = MaxHistoryLengthSetting };
    }

    private void WorldHook_worldUnloading()
    {
        SelectionClipboard = null;
        SelectionHistory = null;
    }

    private void InitializeKeybinds()
    {
        FirstPersonInteraction.RegisterBuildingKeybinding(
                Triggers.AddAlternateSelection,
                AddAlternateSelection.TryOperation,
                true
            );
        FirstPersonInteraction.RegisterBuildingKeybinding(
                Triggers.ClearAlternateSelection,
                ClearAlternateSelection.TryOperation,
                true
            );
        FirstPersonInteraction.RegisterBuildingKeybinding(
                Triggers.ClearSelectionHistory,
                ClearSelectionHistory.TryOperation,
                true
            );
        FirstPersonInteraction.RegisterBuildingKeybinding(
                Triggers.LoadAlternateSelection,
                LoadAlternateSelection.TryOperation,
                true
            );
        FirstPersonInteraction.RegisterBuildingKeybinding(
                Triggers.RedoSelection,
                RedoSelection.TryOperation,
                true
            );
        FirstPersonInteraction.RegisterBuildingKeybinding(
                Triggers.RemoveAlternateSelection,
                RemoveAlternateSelection.TryOperation,
                true
            );
        FirstPersonInteraction.RegisterBuildingKeybinding(
                Triggers.StoreAlternateSelection,
                StoreAlternateSelection.TryOperation,
                true
            );
        FirstPersonInteraction.RegisterBuildingKeybinding(
                Triggers.SwapAlternateSelection,
                SwapAlternateSelection.TryOperation,
                true
            );
        FirstPersonInteraction.RegisterBuildingKeybinding(
                Triggers.UndoSelection,
                UndoSelection.TryOperation,
                true
            );
    }
}

[HarmonyPatch]
class MultiSelectAddAddressPatch
{
    [HarmonyPatch("MultiSelector", "AddToSelectionAndOutline")]
    [HarmonyPrefix]
    static void Prefix(ComponentAddress cAddress)
    {
        MyClient.SelectionHistory.AddCommand(new RemoveComponentFromSelection(cAddress));
    }
}

[HarmonyPatch]
class MultiSelectRemoveAddressPatch
{
    [HarmonyPatch("MultiSelector", "RemoveFromSelectionAndRemoveOutline")]
    [HarmonyPrefix]
    static void Prefix(ComponentAddress cAddress)
    {
        MyClient.SelectionHistory.AddCommand(new AddComponentToSelection(cAddress));
    }
}

[HarmonyPatch]
class MultiSelectOnExitPatch
{
    [HarmonyPatch("MultiSelectState", "OnExit")]
    [HarmonyPrefix]
    static void Prefix()
    {
        var selection = MultiSelector.GetCurrentSelection();
        if (selection == null || selection.Count == 0) 
            return;

        MyClient.SelectionHistory.AddCommand(new RestoreSelection(selection));
    }
}

[HarmonyPatch]
class MultiSelectStartWithSelectionPatch
{
    [HarmonyPatch("MultiSelector", "StartWithSelection")]
    [HarmonyPrefix]
    static void Prefix(ComponentSelection selection)
    {
        MyClient.SelectionHistory.AddCommand(new RestoreSelection());
    }
}

[HarmonyPatch]
class MultiSelectStartSelectingWithPatch
{
    [HarmonyPatch("MultiSelector", "StartSelectingWith")]
    [HarmonyPrefix]
    static void Prefix(ComponentAddress firstComponentInSelection)
    {
        MyClient.SelectionHistory.AddCommand(new RemoveComponentFromSelection(firstComponentInSelection));
    }
}