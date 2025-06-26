namespace Shimmer.Wpf.Events;

public class AllShimmeringStoppedEventArgs : EventArgs
{
    public int Count { get; }

    public AllShimmeringStoppedEventArgs(int count)
    {
        Count = count;
    }
}
