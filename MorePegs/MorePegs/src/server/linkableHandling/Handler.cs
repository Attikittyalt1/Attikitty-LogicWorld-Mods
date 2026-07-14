using LogicAPI.Data;
using System;
using System.Collections.Generic;

namespace MorePegs.LogicCode.LinkableHandling;

public class Handler
{
    public LinkableContainer Linkable { get; init; }
    public Func<ComponentAddress> GetAddress { get; init; }

    private ComponentAddress? _trackerKey;
    private bool _isTracked = false;
    private readonly List<PackageManager> _currentManagers = [];

    public bool IsBeingTracked()
    {
        return _isTracked;
    }

    public void StartTracking(IEnumerable<PackageManager> packageManagers)
    {
        if (_isTracked)
        {
            throw new Exception("Tried to start tracking link that is already being tracked");
        }

        _trackerKey = GetAddress();

        foreach (var manager in packageManagers)
        {
            manager.StartTrackingLinkable(Linkable, _trackerKey.Value);
            _currentManagers.Add(manager);
        }

        _isTracked = true;
    }

    public void StopTracking()
    {
        if (!_isTracked)
        {
            throw new Exception("Tried to stop tracking link that is not being tracked");
        }

        foreach (var manager in _currentManagers)
        {
            manager.StopTrackingLinkable(Linkable, _trackerKey.Value);
        }
        _currentManagers.Clear();

        _trackerKey = null;

        _isTracked = false;
    }
}