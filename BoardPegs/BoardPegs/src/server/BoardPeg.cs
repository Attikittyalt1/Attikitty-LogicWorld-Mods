using BoardPegs.Server;
using LogicAPI.Data;
using LogicAPI.Server.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BoardPegs.Logic.BoardPegHandling;
using JimmysUnityUtilities;

namespace BoardPegs.Logic;

public abstract class BoardPeg : LogicComponent
{
    public readonly static PackageManager2D ManagerAtBoardHeight = new();
    public readonly static PackageManager2D ManagerAboveBoard = new();
    public readonly static PackageManager2D ManagerBelowBoard = new();

    private readonly static IEnumerable<string> ID_CIRCUITBOARDS = ["MHG.CircuitBoard"];

    protected const float Epsilon = 0.01f;

    private Vector3 previousLocation;
    private Handler<Linkable2D> _handler;

    private ComponentAddress GetLinkingAddress()
    {
        return Component.Parent;
    }

    private bool HasBeenMoved()
    {
        return !Component.WorldPosition.IsPrettyCloseTo(previousLocation);
    }

    protected virtual List<PackageManager2D> FindManagers()
    {
        return [ManagerAtBoardHeight];
    }

    protected virtual Vector2Int GetLinkingPosition()
    {
        return new Vector2Int((Component.LocalPositionFixed.x - 50) / 100, (Component.LocalPositionFixed.z - 50) / 100);
    }

    protected abstract bool ShouldBeLinkedHorizontally();

    protected abstract bool ShouldBeLinkedVertically();
     
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
        _handler = new Handler<Linkable2D>
        {
            GetAddress = () => GetLinkingAddress(),
            Linkable = new Linkable2D
            {
                Address = Address,
                LinkablePeg = Inputs[0],
                GetLinkingPosition = GetLinkingPosition,
                HasBeenMoved = HasBeenMoved,
                ShouldBeLinkedHorizontally = ShouldBeLinkedHorizontally,
                ShouldBeLinkedVertically = ShouldBeLinkedVertically
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
