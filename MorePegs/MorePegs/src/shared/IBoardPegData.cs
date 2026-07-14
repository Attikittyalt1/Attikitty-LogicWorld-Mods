
namespace MorePegs.Shared;

public interface IBoardPegData
{
    (bool x, bool y) ConnectedAxis { get; set; }
}