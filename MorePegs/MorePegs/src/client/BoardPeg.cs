using LogicWorld.Rendering.Components;
using MorePegs.Shared;

namespace MorePegs.Client;

public class BoardPeg : ComponentClientCode<IBoardPegData>
{
    public BoardPegPrefabInfo PrefabInfo;

    protected override void SetDataDefaultValues()
    {
        Data.ConnectedAxisX = CodeInfoBools[0];
        Data.ConnectedAxisZ = CodeInfoBools[1];
    }

    protected override void Initialize()
    {
        PrefabInfo = new()
        {
            BlockWidth = CodeInfoFloats[0],
            BlockHeight = CodeInfoFloats[1],
            PegHeight = CodeInfoFloats[2],
            RectSize = CodeInfoFloats[3]
        };
    }

    protected override void DataUpdate()
    {
        var axis = (Data.ConnectedAxisX, Data.ConnectedAxisZ);

        for (int i = 0; i < InputCount; i++)
        {
            var panelPositions = PrefabInfo.GetBlockPanelPositions(i, axis);

            foreach (var (index, pos) in panelPositions)
            {
                SetBlockPosition(index, pos);
            }
        }
    }
}