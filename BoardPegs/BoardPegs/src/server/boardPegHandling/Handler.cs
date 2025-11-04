using LogicAPI.Data;
using System;
using System.Collections.Generic;

namespace BoardPegs.Logic.BoardPegHandling;

public class Handler<T>
{
    public T Linkable { get; init; }
    public Func<ComponentAddress> GetAddress { get; init; }

    private ComponentAddress? _trackerKey;
    private bool _isTracked = false;
    private List<IPackageManager<T>> _currentManagers = [];

    public bool IsBeingTracked()
    {
        return _isTracked;
    }

    public void StartTracking(IEnumerable<IPackageManager<T>> packageManagers)
    {
        if (_isTracked)
        {
            throw new Exception("Tried to start tracking link that is already being tracked");
        }

        _trackerKey = GetAddress();

        foreach (var manager in packageManagers)
        {
            manager.StartTrackingBoardPeg(Linkable, _trackerKey.Value);
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
            manager.StopTrackingBoardPeg(Linkable, _trackerKey.Value);
        }
        _currentManagers.Clear();

        _trackerKey = null;

        _isTracked = false;
    }

    public bool TryStartTracking(IEnumerable<IPackageManager<T>> packageManagers)
    {
        if (!IsBeingTracked())
        {
            StartTracking(packageManagers);
        }

        return false;
    }

    public bool TryStopTracking()
    {
        if (IsBeingTracked())
        {
            StopTracking();
        }

        return false;
    }
}