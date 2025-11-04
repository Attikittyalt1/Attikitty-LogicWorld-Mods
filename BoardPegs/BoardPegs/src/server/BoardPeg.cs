using BoardPegs.Server;
using LogicAPI.Data;
using LogicAPI.Server.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BoardPegs.Logic.BoardPegLink;
using LICC;

namespace BoardPegs.Logic;

public abstract class BoardPeg : LogicComponent
{
    public readonly static PackageManager2D manager = new();

    private readonly static IEnumerable<string> ID_CIRCUITBOARDS = ["MHG.CircuitBoard"];

    protected float Epsilon = 0.01f;

    private Handler<LinkWrapper2D> _handler;

    private ComponentAddress GetLinkingAddress()
    {
        return Component.Parent;
    }

    protected virtual Vector2Int GetLinkingPosition()
    {
        return new Vector2Int((Component.LocalPositionFixed.x - 50) / 100, (Component.LocalPositionFixed.z - 50) / 100);
    }

    protected abstract bool ShouldBeLinkedHorizontally();

    protected abstract bool ShouldBeLinkedVertically();

    private void LinkPeg(IInputPeg peg)
    {
        Inputs[0].AddSecretLinkWith(peg);
    }

    private void UnlinkPeg(IInputPeg peg)
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

    protected override void Initialize()
    {
        _handler = new Handler<LinkWrapper2D>
        {
            GetAddress = () => GetLinkingAddress(),
            PackageManager = manager,
            Link = new LinkWrapper2D
            {
                Address = Address,
                ShouldBeLinkedHorizontally = ShouldBeLinkedHorizontally,
                ShouldBeLinkedVertically = ShouldBeLinkedVertically,
                GetLinkingPosition = GetLinkingPosition,
                LinkPeg = LinkPeg,
                UnlinkPeg = UnlinkPeg
            }
        };
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
            _handler.TryStartTracking();
        }
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
