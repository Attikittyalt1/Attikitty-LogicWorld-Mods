using HarmonyLib;
using LICC;
using LogicAPI.Server;
using System;

namespace MorePegs.Server;

public class MyServer : ServerMod
{
    public const bool DEBUG = false;

    protected override void Initialize()
    {
        var harmony = new Harmony(Manifest.ID);
        harmony.PatchAll();

        ILogicComponentHooks.Init();

        LConsole.WriteLine(String.Format("{0} harmony initialized.", Manifest.ID));
    }
}