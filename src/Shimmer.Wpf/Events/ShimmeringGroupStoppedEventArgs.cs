namespace Shimmer.Wpf.Events;

public class ShimmeringGroupStoppedEventArgs : EventArgs
{
    public string GroupName { get; }
    public int Count { get; }

    public ShimmeringGroupStoppedEventArgs(string groupName, int count)
    {
        GroupName = groupName;
        Count = count;
    }
}
