namespace Shimmer.Wpf.Events;

public class AllShimmeringStartedEventArgs : EventArgs
{
    public int Count { get; }

    public AllShimmeringStartedEventArgs(int count)
    {
        Count = count;
    }
}
