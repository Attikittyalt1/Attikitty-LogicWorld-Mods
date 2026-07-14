using EccsLogicWorldAPI.Server;
using LogicAPI.Server;
using LogicAPI.Services;
using LogicAPI.Server.Components;
using LogicWorld.Server;
using LogicWorld.Server.Circuitry;
using LogicWorld.Server.Modules;
using LogicWorld.SharedCode.Components;
using System;
using EccsLogicWorldAPI.Shared.AccessHelper;
using LogicAPI.Data;
using HarmonyLib;
using LogicAPI.WorldDataMutations;

namespace MorePegs.Server;

public class MyServer : ServerMod
{
    public const bool DEBUG = false;

    protected override void Initialize()
    {
        var harmony = new Harmony("AttikittySelectionToolsClient");
        harmony.PatchAll();

        ILogicComponentHooks.Init();
    }
}