using LogicAPI.Data;
using System.Collections.Generic;

namespace MorePegs.LogicCode.LinkableHandling;

public class Handler2D
{
    public record HandlerInfo2D(ComponentAddress Address, (IEnumerable<PackageManager> x, IEnumerable<PackageManager> y) ValidManagers, (LinkableContainer x, LinkableContainer y) Linkable)
    {
        public void Deconstruct(out Handler.HandlerInfo x, out Handler.HandlerInfo y)
        {
            x = new Handler.HandlerInfo(Address, ValidManagers.x, Linkable.x);
            y = new Handler.HandlerInfo(Address, ValidManagers.y, Linkable.y);
        }
    }

    private readonly Handler _x = new();
    private readonly Handler _y = new();

    public (bool x, bool y) IsBeingTracked { get => (_x.IsBeingTracked, _y.IsBeingTracked); }

    public (List<PackageManager> x, List<PackageManager> y) ActiveManagers { get => (_x.ActiveManagers, _y.ActiveManagers); }

    public void StartTracking(HandlerInfo2D info)
    {
        var (x, y) = info;
        _x.StartTracking(x);
        _y.StartTracking(y);
    }

    public void StopTracking()
    {
        _x.StopTracking(false);
        _y.StopTracking(false);
    }

    public void UpdateTracking(HandlerInfo2D info, bool updatePreviousManagers = true)
    {
        var (x, y) = info;
        _x.UpdateTracking(x, updatePreviousManagers);
        _y.UpdateTracking(y, updatePreviousManagers);
    }
}