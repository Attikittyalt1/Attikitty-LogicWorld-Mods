using JECS;
using JimmysUnityUtilities;
using LogicAPI.Data;
using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.BinaryStuff;
using LogicWorld.SharedCode.Components;
using System.Collections.Generic;
using UnityEngine;
using LICC;

namespace MorePegs.Client;


public class MultiPegPrefabGenerator : DynamicPrefabGenerator<int>
{
    public Color24 BlockColor = Color24.White;
    public float BlockWidth;
    public float BlockHeight;
    public float PegHeight;
    public int DefaultInputCount;

    public float CombinedHeight => BlockHeight + PegHeight;

    public override void Setup(ComponentInfo info)
    {
        DefaultInputCount = info.CodeInfoInts[0];
        BlockWidth = info.CodeInfoFloats[0];
        BlockHeight = info.CodeInfoFloats[1];
        PegHeight = info.CodeInfoFloats[2];
    }

    protected override int GetIdentifierFor(ComponentData componentData)
        => componentData.InputCount;

    public override (int inputCount, int outputCount) GetDefaultPegCounts()
        => (DefaultInputCount, 0);

    protected override Prefab GeneratePrefabFor(int inputCount)
    {
        var blocks = new Block[inputCount];
        var inputs = new ComponentInput[inputCount];

        for (int i = 0; i < inputCount; i++)
        {
            blocks[i] = new Block()
            {
                RawColor = BlockColor,
                Position = new Vector3(0, CombinedHeight * i, 0),
                Scale = new Vector3(BlockWidth, BlockHeight, BlockWidth)
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
}