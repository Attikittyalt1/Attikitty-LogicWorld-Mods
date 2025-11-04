using LogicAPI.Data;

namespace BoardPegs.Logic.BoardPegLink;

public interface IPackageManager<T>
{
    public void StartTrackingBoardPeg(T link, ComponentAddress address);
    public void StopTrackingBoardPeg(T link, ComponentAddress address);
}