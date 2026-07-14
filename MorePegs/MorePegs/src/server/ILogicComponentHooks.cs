using EccsLogicWorldAPI.Server;
using EccsLogicWorldAPI.Shared.AccessHelper;
using HarmonyLib;
using LICC;
using LogicAPI.Data;
using LogicAPI.Server;
using LogicAPI.Server.Components;
using LogicAPI.Services;
using LogicWorld.Server;
using LogicWorld.Server;
using LogicWorld.Server.Circuitry;
using LogicWorld.Server.Managers;
using LogicWorld.Server.Modules;
using LogicWorld.SharedCode.Components;
using SkysGeneralLib.Server.TypeExtensions;
using System;
using System.Reflection;

namespace MorePegs.Server;

public interface ILogicComponentHooks
{
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

        LConsole.WriteLine("LC hooks initialized");
    }
}