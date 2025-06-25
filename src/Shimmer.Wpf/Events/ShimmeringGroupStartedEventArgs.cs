namespace Shimmer.Wpf.Events;

public class ShimmeringGroupStartedEventArgs : EventArgs
{
    public string GroupName { get; }
    public int Count { get; }

    public ShimmeringGroupStartedEventArgs(string groupName, int count)
    {
        GroupName = groupName;
        Count = count;
    }
}
