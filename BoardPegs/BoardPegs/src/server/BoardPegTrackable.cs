using BoardPegs.Server;
using LogicAPI.Data;
using LogicAPI.Server.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace BoardPegs.Logic;

public abstract class BoardPegTrackable : LogicComponent, IBoardPegTrackable
{
    public readonly static BoardPegTracker<ComponentAddress> PrimaryBoardPegTracker = new();
    public readonly static BoardPegTracker<ComponentAddress> SecondaryBoardPegTracker = new();

    private readonly static IEnumerable<string> ID_CIRCUITBOARDS = ["MHG.CircuitBoard"];

    protected float Epsilon = 0.01f;

    private BoardPegTracker<ComponentAddress> _lastBoardPegTracker;
    private ComponentAddress? _trackerKey;
    private bool _isTracked = false;

    protected virtual ComponentAddress GenerateTrackerKey() => Component.Parent;

    protected virtual BoardPegTracker<ComponentAddress> GetBoardPegTracker()
    {
        return PrimaryBoardPegTracker;
    }

    public virtual Vector2Int GetLinkingPosition()
    {
        return new Vector2Int((Component.LocalPositionFixed.x - 50) / 100, (Component.LocalPositionFixed.z - 50) / 100);
    }

    public abstract bool ShouldBeLinkedHorizontally();

    public abstract bool ShouldBeLinkedVertically();

    public void LinkPeg(IInputPeg peg)
    {
        Inputs[0].AddSecretLinkWith(peg);
    }

    public void UnlinkPeg(IInputPeg peg)
    {
        Inputs[0].RemoveSecretLinkWith(peg);
    }

    public bool IsOnValidBoard()
    {
        var parent = GetParentComponent();

        return parent != null && IsCircuitBoard(parent.Data.Type);
    }

    public bool IsAlignedToBoard()
    {
        return true; // throw new NotImplementedException();
    }

    public override void OnComponentDestroyed()
    {
        TryStopTracking();
    }

    public override void OnComponentMoved()
    {
        TryStopTracking();

        if (IsOnValidBoard() && IsAlignedToBoard())
        {
            TryStartTracking();
        }
    }

    public void TryStartTracking()
    {
        if (_isTracked) return;

        _lastBoardPegTracker = GetBoardPegTracker();

        _trackerKey = GenerateTrackerKey();
        _lastBoardPegTracker.StartTrackingBoardPeg(this, _trackerKey.Value);

        _isTracked = true;
    }

    public void TryStopTracking()
    {
        if (!_isTracked) return;

        _lastBoardPegTracker.StopTrackingBoardPeg(this, _trackerKey.Value);
        _trackerKey = null;

        _isTracked = false;
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
