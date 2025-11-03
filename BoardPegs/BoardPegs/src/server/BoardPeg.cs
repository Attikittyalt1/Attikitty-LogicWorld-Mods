using BoardPegs.Server;
using LogicAPI.Data;
using LogicAPI.Server.Components;
using UnityEngine;

namespace BoardPegs.Logic;

public abstract class BoardPeg : LogicComponent, IBoardPeg
{
    protected float MinimumValue = 0.01f;

    public ComponentAddress? AssignedTrackerAddress { set; get; }

    public ComponentAddress GenerateTrackerAddress() => Component.Parent;

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

    private bool IsOnBoard()
    {
        var parent = GetParentComponent();
        return parent != null && GetParentComponent().Data.Type.NumericID == MyServer.ComponentTypesManager.GetNumericID("MHG.CircuitBoard");
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
        if (AssignedTrackerAddress.HasValue)
        {
            BoardPegTracker.StopTrackingBoardPeg(this);
        }
    }

    public override void OnComponentMoved()
    {
        if (AssignedTrackerAddress.HasValue)
        {
            BoardPegTracker.StopTrackingBoardPeg(this);
        }

        if (IsOnBoard() && IsAlignedToBoard())
        {
            BoardPegTracker.StartTrackingBoardPeg(this);
        }
    }
}
