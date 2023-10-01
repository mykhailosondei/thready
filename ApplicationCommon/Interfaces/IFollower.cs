using System.Runtime.CompilerServices;

namespace ApplicationCommon.Interfaces;

public interface IFollower
{
    public List<int> FollowingIds { get; set; }
}