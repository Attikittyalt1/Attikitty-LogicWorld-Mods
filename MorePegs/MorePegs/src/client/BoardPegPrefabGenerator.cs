using LogicAPI.Data;
using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;

namespace MorePegs.Client;


public class BoardPegPrefabGenerator : DynamicPrefabGenerator<int>
{
    public BoardPegPrefabInfo PrefabInfo;
    public (bool x, bool y) DefaultConnectedAxis;
    public int DefaultInputCount;

    public override void Setup(ComponentInfo info)
    {
        DefaultInputCount = info.CodeInfoInts[0];
        DefaultConnectedAxis = (info.CodeInfoBools[0], info.CodeInfoBools[1]);

        PrefabInfo = new()
        {
            BlockWidth = info.CodeInfoFloats[0],
            BlockHeight = info.CodeInfoFloats[1],
            PegHeight = info.CodeInfoFloats[2],
            RectSize = info.CodeInfoFloats[3],
        };
    }

    protected override int GetIdentifierFor(ComponentData componentData)
        => componentData.InputCount;

    public override (int inputCount, int outputCount) GetDefaultPegCounts()
        => (DefaultInputCount, 0);

    protected override Prefab GeneratePrefabFor(int inputCount) => PrefabInfo.GetPrefab(inputCount, DefaultConnectedAxis);
}