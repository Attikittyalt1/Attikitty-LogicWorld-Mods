using HarmonyLib;
using LogicAPI.Data;
using LogicAPI.Services;
using LogicWorld.Server;
using SkysGeneralLib.Server.TypeExtensions;
using System;

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