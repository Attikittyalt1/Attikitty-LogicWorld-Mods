using LogicWorld.ClientCode.Resizing;
using LogicWorld.Interfaces.Building;
using LogicWorld.Rendering.Components;
using System.Linq;
using UnityEngine;

namespace HoverPads.Client;

public class HoverPad : ComponentClientCode<HoverPad.IData>, IResizableY
{
    public interface IData
    {
        int SizeY { get; set; }
    }

    public int SizeY
    {
        get => Data.SizeY;
        set => Data.SizeY = value;
    }

    public int MinY => 2;
    public int MaxY => 6;

    public float GridIntervalY => 1/2f;

    private float Height => Data.SizeY * GridIntervalY;

    private int firstBlockIndex = 1;

    // i should probably calculate the rotation from the direction
    public (Vector3 position, Vector3 direction, Vector3 rotation)[] Points =>
    [
        (
            new Vector3(0f, Height, -0.5f),
            new Vector3(0f, 0f, 1f),
            new Vector3(90f, 0f, 0f)
        )
    ];

    protected override void DataUpdate()
    {
        int blockIndex = firstBlockIndex;

        foreach (var (position, _, rotation) in Points)
        {
            SetBlockPosition(blockIndex, position);
            SetBlockRotation(blockIndex, rotation);
            blockIndex++;
        }

        MarkChildPlacementInfoDirty();
    }

    protected override void SetDataDefaultValues()
    {
        Data.SizeY = 2;
    }

    protected override ChildPlacementInfo GenerateChildPlacementInfo()
    {
        return new ()
        {
            Points = [.. Points.Select((data) => new FixedPlacingPoint
            {
                Position = data.position,
                UpDirection = data.direction
            })]
        };
    }
}