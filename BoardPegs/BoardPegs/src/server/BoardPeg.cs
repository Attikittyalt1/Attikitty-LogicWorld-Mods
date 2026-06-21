using BoardPegs.Server;
using LogicAPI.Data;
using LogicAPI.Server.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BoardPegs.LogicCode.LinkableHandling;
using JimmysUnityUtilities;

namespace BoardPegs.LogicCode;

public abstract class BoardPeg : LogicComponent
{
    public readonly static PackageManager2D<LinkablePeg> ManagerAtBoardHeight = new();
    public readonly static PackageManager2D<LinkablePeg> ManagerAboveBoard = new();
    public readonly static PackageManager2D<LinkablePeg> ManagerBelowBoard = new();

    private readonly static IEnumerable<string> ID_CIRCUITBOARDS = ["MHG.CircuitBoard"];

    protected const float Epsilon = 0.01f;

    private Vector3 previousLocation;
    private Handler2D<LinkablePeg> _handler;

    private ComponentAddress GetLinkingAddress()
    {
        return Component.Parent;
    }

    private bool HasBeenMoved()
    {
        return !Component.WorldPosition.IsPrettyCloseTo(previousLocation);
    }

    protected virtual List<PackageManager2D<LinkablePeg>> FindManagers()
    {
        return [ManagerAtBoardHeight];
    }

    protected virtual Vector2Int GetLinkingPosition()
    {
        return new Vector2Int((Component.LocalPositionFixed.x - 50) / 100, (Component.LocalPositionFixed.z - 50) / 100);
    }

    protected abstract (bool x, bool y) GetAxisStatus();
     
    public bool IsOnValidBoard()
    {
        var parent = GetParentComponent();

        return parent != null && IsCircuitBoard(parent.Data.Type);
    }

    public bool IsAlignedToBoard()
    {
        return true; // throw new NotImplementedException();
    }

    protected override void Initialize()
    {
        _handler = new Handler2D<LinkablePeg>
        {
            GetAddress = () => GetLinkingAddress(),
            Linkable = new LinkableContainer2D<LinkablePeg>
            {
                Address = Address,
                Linkable = new LinkablePeg(Inputs[0]),
                GetLinkingPosition = GetLinkingPosition,
                GetAxisStatus = GetAxisStatus,
                HasBeenMoved = HasBeenMoved,
            }
        };

        previousLocation = Component.WorldPosition;
    }

    public override void OnComponentDestroyed()
    {
        _handler.TryStopTracking();
    }

    public override void OnComponentMoved()
    {
        _handler.TryStopTracking();

        if (IsOnValidBoard() && IsAlignedToBoard())
        {
            _handler.TryStartTracking(FindManagers());
        }

        previousLocation = Component.WorldPosition;
    }

    private IComponentInWorld GetParentComponent()
    {
        return MyServer.WorldData.Lookup(Component.Parent);
    }

    private static bool IsCircuitBoard(ComponentType type)
    {
        return ID_CIRCUITBOARDS.Any(id => type.NumericID == MyServer.ComponentTypesManager.GetNumericID(id));
    }
}
