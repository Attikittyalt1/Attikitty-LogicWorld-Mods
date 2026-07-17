using JimmysUnityUtilities;
using LICC;
using LogicAPI.Data;
using MorePegs.Server;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MorePegs.LogicCode.LinkableHandling;

public class Handler
{
    public bool IsBeingTracked { get; private set; } = false;
    public List<PackageManager> ActiveManagers { get; } = [];

    public required Func<ComponentAddress> GetAddress { get; init; }
    public required Func<LinkableContainer> GetLinkable { get; init; }

    private ComponentAddress? _trackerKey;
    private LinkableContainer _linkable;

    public void StartTracking(IEnumerable<PackageManager> packageManagers, bool skipIfIfZeroManagers = true, bool acknowledgePreviousState = true)
    {
        if (IsBeingTracked)
        {
            if (acknowledgePreviousState)
            {
                throw new Exception("Tried to start tracking link that is already being tracked");
            }
        }

        if (skipIfIfZeroManagers && packageManagers.IsEmpty())
        {
            return;
        } 

        if (IsBeingTracked == false)
        {
            _trackerKey = GetAddress();
            _linkable = GetLinkable();
        }

        foreach (var manager in packageManagers)
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

    public void UpdateTracking(IEnumerable<PackageManager> packageManagers, bool updatePreviousManagers = true)
    {
        if (updatePreviousManagers || packageManagers.IsEmpty())
        {
            StopTracking(false);
        } 
        else
        {
            foreach (var manager in ActiveManagers)
            {
                if (!packageManagers.Contains(manager))
                {
                    manager.StopTrackingLinkable(_linkable, _trackerKey.Value);
                    ActiveManagers.Remove(manager);
                }
            }
        }

        StartTracking(packageManagers, true, updatePreviousManagers); // technically I could just put false for acknowledgePreviousState but this makes things a bit nicer
    }
}