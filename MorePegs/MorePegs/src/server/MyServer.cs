using HarmonyLib;
using LICC;
using LogicAPI.Server;
using MorePegs.LogicCode.LinkableHandling;

namespace MorePegs.Server;

public class MyServer : ServerMod
{
    public const bool DEBUG = false;

    public static readonly (PackageManager x, PackageManager y) ManagersAboveBoard = (new(), new());
    public static readonly (PackageManager x, PackageManager y) ManagersBelowBoard = (new(), new());

    protected override void Initialize()
    {
        var harmony = new Harmony(Manifest.ID);
        harmony.PatchAll();

        ILogicComponentHooks.Init();

        LConsole.WriteLine("{0} harmony initialized.", Manifest.ID);
    }
}