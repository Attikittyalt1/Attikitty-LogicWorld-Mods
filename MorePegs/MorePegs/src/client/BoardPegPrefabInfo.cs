using JimmysUnityUtilities;
using LogicWorld.References;
using LogicWorld.SharedCode.Components;
using UnityEngine;

namespace MorePegs.Client;

public class BoardPegPrefabInfo()
{
    public readonly Color24 BlockColor = Color24.White;
    public readonly Color24 RectColor = Color24.OrangeCrayola;

    public float BlockWidth { get; init; }
    public float BlockHeight { get; init; }
    public float PegHeight {  get; init; }
    public float RectSize {  get; init; }

    public float CombinedHeight => BlockHeight + PegHeight;
    public float RectWidthOffset => BlockWidth / 2 + 0.001f;
    public float RectHeightOffset => (BlockHeight - RectSize) * 3;

    public Prefab GetPrefab(int InputCount, (bool x, bool y) ConnectedAxis)
    {
        var blocks = new Block[5 * InputCount];
        var inputs = new ComponentInput[InputCount];

        for (int i = 0; i < InputCount; i++)
        {
            blocks[5 * i] = new Block()
            {
                RawColor = BlockColor,
                Position = new Vector3(0, CombinedHeight * i, 0),
                Scale = new Vector3(BlockWidth, BlockHeight, BlockWidth),
                ColliderData = new ColliderData()
                {
                    Type = ColliderType.None
                }
            };

            blocks[5 * i + 1] = new Block()
            {
                RawColor = RectColor,
                Position = new Vector3(ConnectedAxis.x ? RectWidthOffset : 0, RectHeightOffset + CombinedHeight * i, 0),
                Rotation = new Vector3(0, 0, -90),
                Scale = new Vector3(RectSize, RectSize, RectSize),
                ColliderData = new ColliderData()
                {
                    Type = ColliderType.None
                },
                Mesh = Meshes.FlatQuad,
            };

            blocks[5 * i + 2] = new Block()
            {
                RawColor = RectColor,
                Position = new Vector3(ConnectedAxis.x ? -RectWidthOffset : 0, RectHeightOffset + CombinedHeight * i, 0),
                Rotation = new Vector3(0, 0, 90),
                Scale = new Vector3(RectSize, RectSize, RectSize),
                ColliderData = new ColliderData()
                {
                    Type = ColliderType.None
                },
                Mesh = Meshes.FlatQuad
            };

            blocks[5 * i + 3] = new Block()
            {
                RawColor = RectColor,
                Position = new Vector3(0, RectHeightOffset + CombinedHeight * i, ConnectedAxis.y ? RectWidthOffset : 0),
                Rotation = new Vector3(90, 0, 0),
                Scale = new Vector3(RectSize, RectSize, RectSize),
                ColliderData = new ColliderData()
                {
                    Type = ColliderType.None
                },
                Mesh = Meshes.FlatQuad
            };

            blocks[5 * i + 4] = new Block()
            {
                RawColor = RectColor,
                Position = new Vector3(0, RectHeightOffset + CombinedHeight * i, ConnectedAxis.y ? -RectWidthOffset : 0),
                Rotation = new Vector3(-90, 0, 0),
                Scale = new Vector3(RectSize, RectSize, RectSize),
                ColliderData = new ColliderData()
                {
                    Type = ColliderType.None
                },
                Mesh = Meshes.FlatQuad
            };

            inputs[i] = new ComponentInput()
            {
                Position = new Vector3(0, BlockHeight + CombinedHeight * i, 0),
                Length = PegHeight
            };
        }

        return new Prefab()
        {
            Blocks = blocks,
            Inputs = inputs
        };
    }

    public (int index, Vector3 pos)[] GetBlockPanelPositions(int Index, (bool x, bool y) ConnectedAxis) => [
        (5 * Index + 1, new Vector3(ConnectedAxis.x ? RectWidthOffset : 0, RectHeightOffset + CombinedHeight * Index, 0)),
        (5 * Index + 2, new Vector3(ConnectedAxis.x ? -RectWidthOffset : 0, RectHeightOffset + CombinedHeight * Index, 0)),
        (5 * Index + 3, new Vector3(0, RectHeightOffset + CombinedHeight * Index, ConnectedAxis.y ? RectWidthOffset : 0)),
        (5 * Index + 4, new Vector3(0, RectHeightOffset + CombinedHeight * Index, ConnectedAxis.y ? -RectWidthOffset : 0))
    ];
}