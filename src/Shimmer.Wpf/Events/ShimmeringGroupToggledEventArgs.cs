namespace Shimmer.Wpf.Events;

public class ShimmeringGroupToggledEventArgs : EventArgs
{
    public string GroupName { get; }
    public int Count { get; }

    public ShimmeringGroupToggledEventArgs(string groupName, int count)
    {
        GroupName = groupName;
        Count = count;
    }
}
