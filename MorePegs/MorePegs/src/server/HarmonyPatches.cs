using HarmonyLib;
using LICC;
using LogicAPI.Data;
using LogicAPI.WorldDataMutations;
using MorePegs.LogicCode.LinkableHandling;
using SkysGeneralLib.Server.TypeExtensions;
using System.Collections.Generic;
using System.Linq;

namespace MorePegs.Server;

[HarmonyPatch]
class HarmonyPatches
{
    [HarmonyPatch("ServerWorldDataMutator", "RepositionComponent")]
    [HarmonyPostfix]
    static void RepositionComponentPatch(WorldMutation_RepositionComponent mutation)
    {
        var parent = mutation.AddressOfTargetComponent;

        List<PackageManager> managersX = [];
        List<PackageManager> managersY = [];

        foreach (var child in parent.GetComponent().EnumerateChildren())
        {
            var component = child.GetLogicComponent();

            (component as ILogicComponentHooks)?.OnParentRepositioned();

            if (component is IHasParentWithPackageManager componentAsIHasParentWithPackageManager)
            {
                var currentManagers = componentAsIHasParentWithPackageManager.GetActiveManagers();

                managersX.AddRange(currentManagers.x.Except(managersX));
                managersY.AddRange(currentManagers.y.Except(managersY));
            }
        }

        var offset = mutation.LocalPositionDelta;
        var fixedOffset = ComponentData.ConvertPositionToFixedPosition(offset);

        var offsetX = -(fixedOffset.x) / 100;
        var offsetY = -(fixedOffset.z) / 100;

        if (MyServer.DEBUG) LConsole.WriteLine("new offset: {0}, {1}", offsetX, offsetY);
        if (MyServer.DEBUG) LConsole.WriteLine("manager counts: {0}, {1}", managersX.Count, managersY.Count);

        foreach (var manager in managersX)
        {
            manager.OffsetPositions(parent, offsetX);
        }

        foreach (var manager in managersY)
        {
            manager.OffsetPositions(parent, offsetY);
        }
    }


    [HarmonyPatch("CircuitryManager", "FullyRemovePegsFromCircuitModel")]
    [HarmonyPrefix]
    static void FullyRemovePegsFromCircuitModelPatch(ComponentAddress cAddress)
    {
        if (cAddress.GetLogicComponent() is not LogicCode.BoardPeg)
        {
            return;
        }

        foreach (var input in cAddress.GetLogicComponent().Inputs)
        {
            input.RemoveAllSecretLinks();
        }
    }
}