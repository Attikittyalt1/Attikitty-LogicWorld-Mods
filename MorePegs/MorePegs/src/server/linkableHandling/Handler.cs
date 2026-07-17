using JimmysUnityUtilities;
using LogicAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MorePegs.LogicCode.LinkableHandling;

public class Handler
{
    public record HandlerInfo(ComponentAddress Address, IEnumerable<PackageManager> ValidManagers, LinkableContainer Linkable)
    {

    }

    public bool IsBeingTracked { get; private set; } = false;
    public List<PackageManager> ActiveManagers { get; } = [];
    private ComponentAddress? _trackerKey;
    private LinkableContainer _linkable;

    public void StartTracking(HandlerInfo info, bool skipIfIfZeroManagers = true, bool acknowledgePreviousState = true)
    {
        if (IsBeingTracked)
        {
            if (acknowledgePreviousState)
            {
                throw new Exception("Tried to start tracking link that is already being tracked");
            }
        }

        var validManagers = info.ValidManagers;

        if (skipIfIfZeroManagers && validManagers.IsEmpty())
        {
            return;
        } 

        if (IsBeingTracked == false)
        {
            _trackerKey = info.Address;
            _linkable = info.Linkable;
        }

        foreach (var manager in validManagers)
        {
            if (acknowledgePreviousState || !ActiveManagers.Contains(manager))
            {
                manager.StartTrackingLinkable(_linkable, _trackerKey.Value);
                ActiveManagers.Add(manager);
            }
        }

        IsBeingTracked = true;
    }

    public void StopTracking(bool failIfAlreadyEmpty = true)
    {
        if (!IsBeingTracked)
        {
            if (failIfAlreadyEmpty)
            {
                throw new Exception("Tried to stop tracking link that is not being tracked");
            }

            return;
        }

        foreach (var manager in ActiveManagers)
        {
            manager.StopTrackingLinkable(_linkable, _trackerKey.Value);
        }

        ActiveManagers.Clear();

        _trackerKey = null;
        _linkable = null;

        IsBeingTracked = false;
    }

    public void UpdateTracking(HandlerInfo info, bool updatePreviousManagers = true)
    {
        var validManagers = info.ValidManagers;

        if (updatePreviousManagers || validManagers.IsEmpty())
        {
            StopTracking(false);
        } 
        else
        {
            foreach (var manager in ActiveManagers)
            {
                if (!validManagers.Contains(manager))
                {
                    manager.StopTrackingLinkable(_linkable, _trackerKey.Value);
                    ActiveManagers.Remove(manager);
                }
            }
        }

        StartTracking(info, true, updatePreviousManagers);
    }
}