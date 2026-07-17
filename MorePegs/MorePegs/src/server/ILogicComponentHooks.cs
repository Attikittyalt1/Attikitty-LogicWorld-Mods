using HarmonyLib;
using LICC;
using LogicAPI.Data;
using LogicAPI.Services;
using LogicAPI.WorldDataMutations;
using LogicWorld.Server;
using MorePegs.LogicCode.LinkableHandling;
using SkysGeneralLib.Server.TypeExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MorePegs.Server;

public interface ILogicComponentHooks
{
    public virtual void OnParentRepositioned()
    {

    }

    public virtual void OnComponentPegCountUpdated()
    {
        
    }

    private static void OnComponentPegCountUpdated(ComponentAddress address)
    {
        (address.GetLogicComponent() as ILogicComponentHooks)?.OnComponentPegCountUpdated();
    }

    public static void Init()
    {
        var type = AccessTools.TypeByName("LogicWorld.Server.Managers.ServerWorldDataMutator");
        var eventInfo = type.GetEvent("OnComponentPegCountUpdated");
        var methodInfo = AccessTools.Method(typeof(ILogicComponentHooks), nameof(OnComponentPegCountUpdated), [typeof(ComponentAddress)]);
        var handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, null, methodInfo);
        //var handler = AccessTools.MethodDelegate<Delegate>(methodInfo);
        eventInfo.AddEventHandler(Program.Get<IWorldDataMutator>(), handler);
    }
}

[HarmonyPatch]
class MultiSelectRemoveAddressPatch
{
    [HarmonyPatch("ServerWorldDataMutator", "RepositionComponent")]
    [HarmonyPostfix]
    static void Postfix(WorldMutation_RepositionComponent mutation)
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
}