using LICC;
using LogicAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MorePegs.LogicCode.LinkableHandling;

public class Handler2D
{
    private Handler _x;
    private Handler _y;

    public Handler2D(Func<ComponentAddress> GetAddress, (Func<LinkableContainer> x, Func<LinkableContainer> y) GetLinkable)
    {
        _x = new Handler()
        {
            GetAddress = GetAddress,
            GetLinkable = GetLinkable.x
        };

        _y = new Handler()
        {
            GetAddress = GetAddress,
            GetLinkable = GetLinkable.y
        };
    }

    public (bool x, bool y) IsBeingTracked { get => (_x.IsBeingTracked, _y.IsBeingTracked); }

    public (List<PackageManager> x, List<PackageManager> y) ActiveManagers { get => (_x.ActiveManagers, _y.ActiveManagers); }

    public void StartTracking((IEnumerable<PackageManager> x, IEnumerable<PackageManager> y) managers, (bool x, bool y) connectedAxis)
    {
        if (connectedAxis.x)
        {
            _x.StartTracking(managers.x);
        }

        if (connectedAxis.y)
        {
            _y.StartTracking(managers.y);
        }
    }

    public void StopTracking()
    {
        if (_x.IsBeingTracked)
        {
            _x.StopTracking();
        }

        if (_y.IsBeingTracked)
        {
            _y.StopTracking();
        }
    }

    public void UpdateTracking((IEnumerable<PackageManager> x, IEnumerable<PackageManager> y) managers, (bool x, bool y) connectedAxis)
    {
        if (connectedAxis.x)
        {
            if (!_x.IsBeingTracked)
            {
                _x.StartTracking(managers.x);
            }
        }
        else
        {
            if (_x.IsBeingTracked)
            {
                _x.StopTracking();
            }
        }

        if (connectedAxis.y)
        {
            if (!_y.IsBeingTracked)
            {
                _y.StartTracking(managers.y);
            }
        }
        else
        {
            if (_y.IsBeingTracked)
            {
                _y.StopTracking();
            }
        }
    }
}