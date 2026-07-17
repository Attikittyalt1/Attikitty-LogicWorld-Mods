using JimmysUnityUtilities;
using LICC;
using LogicAPI;
using LogicAPI.Data;
using LogicWorld.Server.Circuitry;
using MorePegs.LogicCode.LinkableHandling;
using MorePegs.Server;
using MorePegs.Shared;
using SkysGeneralLib.Server.TypeExtensions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

namespace MorePegs.LogicCode;

public class BoardPeg : LogicComponent<IBoardPegData>, ILogicComponentHooks, IHasParentWithPackageManager
{
    protected const float Epsilon = 0.01f;

    private (int x, int y) oldPosition;
    private bool _initalizing;
    private Handler2D _handler;
    private ILinkable _linkable;

    public ComponentAddress GetLinkingAddress() => Component.Parent;

    public (List<PackageManager> x, List<PackageManager> y) GetManagers()
    {
        List<(PackageManager, PackageManager)> managers = (Component.LocalPositionFixed.y - 75) switch
        {
            > 0 => [MyServer.ManagersAboveBoard],
            < 0 => [MyServer.ManagersBelowBoard],
            _ => [MyServer.ManagersAboveBoard, MyServer.ManagersBelowBoard]
        };
        
        return managers.Unpack();
    }

    public (List<PackageManager> x, List<PackageManager> y) GetActiveManagers() => _handler.ActiveManagers;

    public (int x, int y) GetLinkingPosition() => (
        (Component.LocalPositionFixed.x - 50) / 100, 
        (Component.LocalPositionFixed.z - 50) / 100
    );

    public (bool x, bool y) GetAxisStatus()
    {
        var newRight = Shared.QuaternionExtensions.FromToRotation(Component.localUp, Vector3.up) * Component.localRight;

        var (parallelX, parallelZ) = (
            Mathf.Abs(newRight.x) > Epsilon,
            Mathf.Abs(newRight.z) > Epsilon
        );

        (bool connectX, bool connectZ) = (Data.ConnectedAxisZ, Data.ConnectedAxisX);

        return (
            connectX && parallelX || connectZ && parallelZ,
            connectX && parallelZ || connectZ && parallelX
        );
    }

    public bool IsOnValidBoard() => 
        GetLinkingAddress().GetComponent() != null;

    protected override void Initialize()
    {
        _linkable = new LinkableMultiPeg(Inputs);

        _handler = new Handler2D(
            GetLinkingAddress, 
            (
                () => new() 
                {
                    Address = Address,
                    Linkable = _linkable,
                    Position = GetLinkingPosition().x
                }, 
                () => new() 
                {
                    Address = Address,
                    Linkable = _linkable,
                    Position = GetLinkingPosition().y
                }
            )
        );

        if (IsOnValidBoard()) {
            _handler.StartTracking(GetManagers(), GetAxisStatus());
            oldPosition = GetLinkingPosition();
        }

        _initalizing = true;
    }

    public override void OnComponentDestroyed()
    {
        _handler.StopTracking();
    }

    public override void OnComponentMoved()
    {
        if (_initalizing)
        {
            _initalizing = false;
            return;
        }

        _handler.StopTracking();
        if (IsOnValidBoard())
        {
            _handler.StartTracking(GetManagers(), GetAxisStatus());
            oldPosition = GetLinkingPosition();
        }
    }

    public virtual void OnParentRepositioned()
    {
        var newPosition = GetLinkingPosition();
        if (MyServer.DEBUG) LConsole.WriteLine("reposition coords: {0}, {1}", newPosition.x - oldPosition.x, newPosition.y - oldPosition.y);
        oldPosition = GetLinkingPosition();
    }

    protected override void OnCustomDataUpdated()
    {
        if (_handler == null) {
            return;
        }

        if (IsOnValidBoard())
        {
            _handler.UpdateTracking(GetManagers(), GetAxisStatus());
        }
    }

    public void OnComponentPegCountUpdated()
    {
        _handler.StopTracking();

        _linkable = new LinkableMultiPeg(Inputs);

        if (IsOnValidBoard())
        {
            _handler.StartTracking(GetManagers(), GetAxisStatus());
        }
    }

    protected override void SetDataDefaultValues()
    {
        Data.ConnectedAxisX = CodeInfoBools[0];
        Data.ConnectedAxisZ = CodeInfoBools[1];
    }
}
