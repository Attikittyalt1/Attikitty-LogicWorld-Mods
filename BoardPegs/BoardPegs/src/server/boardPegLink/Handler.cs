using LogicAPI.Data;
using System;

namespace BoardPegs.Logic.BoardPegLink;

public class Handler<T>
{
    public T Link { get; init; }
    public IPackageManager<T> PackageManager { get; init; }
    public Func<ComponentAddress> GetAddress { get; init; }

    private ComponentAddress? _trackerKey;
    private bool _isTracked = false;

    public bool IsBeingTracked()
    {
        return _isTracked;
    }

    public void StartTracking()
    {
        if (_isTracked)
        {
            throw new Exception("Tried to start tracking link that is already being tracked");
        }

        _trackerKey = GetAddress();
        PackageManager.StartTrackingBoardPeg(Link, _trackerKey.Value);

        _isTracked = true;
    }

    public void StopTracking()
    {
        if (!_isTracked)
        {
            throw new Exception("Tried to stop tracking link that is not being tracked");
        }

        PackageManager.StopTrackingBoardPeg(Link, _trackerKey.Value);
        _trackerKey = null;

        _isTracked = false;
    }

    public bool TryStartTracking()
    {
        if (!IsBeingTracked())
        {
            StartTracking();
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