using JimmysUnityUtilities;
using LogicAPI.Data;
using LogicWorld.Server.Circuitry;
using MorePegs.LogicCode.LinkableHandling;
using MorePegs.Server;
using MorePegs.Shared;
using SkysGeneralLib.Server.TypeExtensions;
using System.Collections.Generic;
using UnityEngine;

namespace MorePegs.LogicCode;

public class BoardPeg : LogicComponent<IBoardPegData>, ILogicComponentHooks
{
    public readonly static PackageManager2D ManagerAtBoardHeight = new();
    public readonly static PackageManager2D ManagerAboveBoard = new();
    public readonly static PackageManager2D ManagerBelowBoard = new();

    protected const float Epsilon = 0.01f;

    private Handler2D _handler;

    private ComponentAddress GetLinkingAddress()
    {
        return Component.Parent;
    }

    protected List<PackageManager2D> FindManagers() => (Component.LocalPositionFixed.y - 75) switch
    {
        > 0 => [ManagerAboveBoard],
        < 0 => [ManagerBelowBoard],
        _ => [ManagerAtBoardHeight, ManagerAboveBoard, ManagerBelowBoard]
    };

    protected virtual Vector2Int GetLinkingPosition()
    {
        return new Vector2Int((Component.LocalPositionFixed.x - 50) / 100, (Component.LocalPositionFixed.z - 50) / 100);
    }

    protected (bool x, bool z) GetAxisStatus() => (
        GetEffectiveAxis().x && (Mathf.Abs(Component.localUp.z) >= Epsilon || Mathf.Abs(Component.localUp.y) >= Epsilon),
        GetEffectiveAxis().z && (Mathf.Abs(Component.localUp.x) >= Epsilon || Mathf.Abs(Component.localUp.y) >= Epsilon)
    );

    private (bool x, bool z) GetEffectiveAxis()
    {
        bool rotated = Mathf.Abs((Component.LocalRotation*Vector3.right).RoundToNearestCardinalValue().x) >= 0.5;

        return rotated ? (Data.ConnectedAxisZ, Data.ConnectedAxisX) : (Data.ConnectedAxisX, Data.ConnectedAxisZ);
    }
    
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
    }

    public override void OnComponentDestroyed()
    {
        TryStopTracking();
    }

    public override void OnComponentMoved()
    {
        TryStopTracking();
        TryStartTracking();
    }

    protected override void OnCustomDataUpdated()
    {
        if (_handler == null)
        {
            return;
        }

        TryStopTracking();
        TryStartTracking();
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
        TryStopTracking();

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

        TryStartTracking();
    }

    private void TryStopTracking()
    {
        if (_handler.IsBeingTracked())
        {
            _handler.StopTracking();
        }
    }

    private void TryStartTracking()
    {
        if (IsOnValidBoard())
        {
            _handler.StartTracking(FindManagers());
        }
    }

    protected override void SetDataDefaultValues()
    {
        Data.ConnectedAxisX = CodeInfoBools[0];
        Data.ConnectedAxisZ = CodeInfoBools[1];
    }
}
