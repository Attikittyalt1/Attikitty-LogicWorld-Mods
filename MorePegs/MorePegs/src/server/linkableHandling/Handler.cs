using LICC;
using LogicAPI.Data;
using System;
using System.Collections.Generic;

namespace MorePegs.LogicCode.LinkableHandling;

public class Handler
{
    public bool IsBeingTracked { get; private set; } = false;
    public List<PackageManager> ActiveManagers { get; } = [];

    public required Func<ComponentAddress> GetAddress { get; init; }
    public required Func<LinkableContainer> GetLinkable { get; init; }

    private ComponentAddress? _trackerKey;
    private LinkableContainer _linkable;

    public void StartTracking(IEnumerable<PackageManager> packageManagers)
    {
        if (IsBeingTracked)
        {
            throw new Exception("Tried to start tracking link that is already being tracked");
        }

        _trackerKey = GetAddress();
        _linkable = GetLinkable();

        foreach (var manager in packageManagers)
        {
            manager.StartTrackingLinkable(_linkable, _trackerKey.Value);
            ActiveManagers.Add(manager);
        }

        IsBeingTracked = true;
    }

    public void StopTracking()
    {
        if (!IsBeingTracked)
        {
            throw new Exception("Tried to stop tracking link that is not being tracked");
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
}