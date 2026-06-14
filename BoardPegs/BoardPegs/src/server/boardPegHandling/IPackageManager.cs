using LogicAPI.Data;

namespace BoardPegs.LogicCode.BoardPegHandling;

public interface IPackageManager<T>
{
    public void StartTrackingBoardPeg(T linkable, ComponentAddress address);
    public void StopTrackingBoardPeg(T linkable, ComponentAddress address);
}