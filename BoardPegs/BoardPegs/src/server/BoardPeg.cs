using BoardPegs.Server;
using LogicAPI.Data;
using LogicAPI.Server.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BoardPegs.Logic;

public abstract class BoardPeg : LogicComponent, IBoardPegTrackable<ComponentAddress>
{
    private readonly static IEnumerable<string> ID_CIRCUITBOARDS = ["MHG.CircuitBoard"];

    public readonly static BoardPegTracker<ComponentAddress> PrimaryBoardPegTracker = new();

    private BoardPegTracker<ComponentAddress> _lastBoardPegTracker;

    protected float Epsilon = 0.01f;

    public ComponentAddress AssignedTrackerKey { set; get; }

    public bool IsTracked { set; get; }

    public ComponentAddress GenerateTrackerKey() => Component.Parent;

    public virtual BoardPegTracker<ComponentAddress> GetBoardPegTracker()
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

    private bool IsOnValidBoard()
    {
        var parent = GetParentComponent();

        return parent != null && IsCircuitBoard(parent.Data.Type);
    }

    private bool IsCircuitBoard(ComponentType type)
    {
        return ID_CIRCUITBOARDS.Any(id => type.NumericID == MyServer.ComponentTypesManager.GetNumericID(id));
    }

    private bool IsAlignedToBoard()
    {
        return true; // throw new NotImplementedException();
    }

    private IComponentInWorld GetParentComponent()
    {
        return MyServer.WorldData.Lookup(Component.Parent);
    }

    public override void OnComponentDestroyed()
    {
        if (IsTracked)
        {
            _lastBoardPegTracker.StopTrackingBoardPeg(this);
        }
    }

    public override void OnComponentMoved()
    {
        if (IsTracked)
        {
            _lastBoardPegTracker.StopTrackingBoardPeg(this);
        }

        if (IsOnValidBoard() && IsAlignedToBoard())
        {
            _lastBoardPegTracker = GetBoardPegTracker();
            _lastBoardPegTracker.StartTrackingBoardPeg(this);
        }
    }
}
