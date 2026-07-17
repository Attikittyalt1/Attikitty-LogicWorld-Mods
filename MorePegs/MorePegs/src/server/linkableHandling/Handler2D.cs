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

    public void StartTracking((IEnumerable<PackageManager> x, IEnumerable<PackageManager> y) managers)
    {
        _x.StartTracking(managers.x);
        _y.StartTracking(managers.y);
    }

    public void StopTracking()
    {
        _x.StopTracking(false);
        _y.StopTracking(false);
    }

    public void UpdateTracking((IEnumerable<PackageManager> x, IEnumerable<PackageManager> y) managers, bool updatePreviousManagers = true)
    {
        _x.UpdateTracking(managers.x, updatePreviousManagers);
        _y.UpdateTracking(managers.y, updatePreviousManagers);
    }
}