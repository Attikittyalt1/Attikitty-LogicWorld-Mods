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

    private bool _initalizing;
    private Handler2D _handler;
    private ILinkable _linkable;

    public static List<(PackageManager x, PackageManager y)> GetManagerPairsGivenHeight(int height) => height switch
    {
        > 0 => [MyServer.ManagersAboveBoard],
        < 0 => [MyServer.ManagersBelowBoard],
        _ => [MyServer.ManagersAboveBoard, MyServer.ManagersBelowBoard]
    };

    public ComponentAddress GetLinkingAddress() => Component.Parent;

    public (int x, int y) GetLinkingPosition() => (
        (Component.LocalPositionFixed.x - 50) / 100,
        (Component.LocalPositionFixed.z - 50) / 100
    );

    public bool IsOnValidBoard() =>
        GetLinkingAddress().GetComponent() != null;

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

    public (List<PackageManager> x, List<PackageManager> y) GetValidManagers()
    {
        if (!IsOnValidBoard())
        {
            return ([], []);
        }

        var managers = GetManagerPairsGivenHeight((Component.LocalPositionFixed.y - 75) / 100).Unpack();
        var axisStatus = GetAxisStatus();

        return (axisStatus.x ? managers.Item1 : [], axisStatus.y ? managers.Item2 : []);
    }

    public (LinkableContainer x, LinkableContainer y) GetLinkableContainers() => (
        new()
        {
            Address = Address,
            Linkable = _linkable,
            Position = GetLinkingPosition().x
        },
        new()
        {
            Address = Address,
            Linkable = _linkable,
            Position = GetLinkingPosition().y
        }
    );

    public Handler2D.HandlerInfo2D GetInfo() => new(GetLinkingAddress(), GetValidManagers(), GetLinkableContainers());

    public (List<PackageManager> x, List<PackageManager> y) GetActiveManagers() => _handler.ActiveManagers;

    protected override void Initialize()
    {
        _linkable = new LinkableMultiPeg(Inputs);

        _handler = new Handler2D();

        _handler.StartTracking(GetInfo());

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

        _handler.UpdateTracking(GetInfo());
    }

    protected override void OnCustomDataUpdated()
    {
        if (_handler == null) {
            return;
        }

        _handler.UpdateTracking(GetInfo(), false);
    }

    public void OnComponentPegCountUpdated()
    {
        _handler.StopTracking();

        _linkable = new LinkableMultiPeg(Inputs);
        
        _handler.StartTracking(GetInfo());
    }

    protected override void SetDataDefaultValues()
    {
        Data.ConnectedAxisX = CodeInfoBools[0];
        Data.ConnectedAxisZ = CodeInfoBools[1];
    }
}
