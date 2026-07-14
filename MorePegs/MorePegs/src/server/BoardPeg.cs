using MorePegs.Server;
using LogicAPI.Data;
using LogicAPI.Server.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MorePegs.LogicCode.LinkableHandling;
using JimmysUnityUtilities;
using LogicWorld.Server.Circuitry;
using MorePegs.Shared;
using LogicWorld.SharedCode.Components;
using LICC;
using SkysGeneralLib.Server.TypeExtensions;

namespace MorePegs.LogicCode;

public abstract class BoardPeg : LogicComponent, ILogicComponentHooks
{
    public readonly static PackageManager2D ManagerAtBoardHeight = new();
    public readonly static PackageManager2D ManagerAboveBoard = new();
    public readonly static PackageManager2D ManagerBelowBoard = new();

    protected const float Epsilon = 0.01f;

    private Vector3 previousLocation;
    private Handler2D _handler;

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

    protected abstract (bool x, bool y) GetAxisStatus();


     
    public bool IsOnValidBoard()
    {
        var parent = Component.Parent.GetComponent();

        return parent != null;
    }

    protected override void Initialize()
    {
        _handler = new Handler2D
        {
            GetAddress = () => GetLinkingAddress(),
            Linkable = new LinkableContainer2D
            {
                Address = Address,
                Linkable = new LinkableMultiPeg(Inputs),
                GetLinkingPosition = GetLinkingPosition,
                GetAxisStatus = GetAxisStatus,
            }
        };

        previousLocation = Component.WorldPosition;
    }

    public override void OnComponentDestroyed()
    {
        _handler.StopTracking();
    }

    public override void OnComponentMoved()
    {
        if (_handler.IsBeingTracked())
        {
            _handler.StopTracking();
        }

        if (IsOnValidBoard())
        {
            _handler.StartTracking(FindManagers());
        }

        previousLocation = Component.WorldPosition;
    }

    public void OnParentRepositioned()
    {
        if (_handler.IsBeingTracked())
        {
            _handler.UpdatePositions();
        }
    }

    public void OnComponentPegCountUpdated()
    {
        _handler.StopTracking();

        _handler = new Handler2D
        {
            GetAddress = () => GetLinkingAddress(),
            Linkable = new LinkableContainer2D
            {
                Address = Address,
                Linkable = new LinkableMultiPeg(Inputs),
                GetLinkingPosition = GetLinkingPosition,
                GetAxisStatus = GetAxisStatus,
            }
        };

        if (IsOnValidBoard())
        {
            _handler.StartTracking(FindManagers());
        }

        previousLocation = Component.WorldPosition;
    }

    /*protected override void SetDataDefaultValues()
    {
        Data.ConnectedAxis = (CodeInfoBools[0], CodeInfoBools[1]);
    }*/
}
