namespace Shimmer.Wpf.Events;

public class AllShimmeringToggledEventArgs : EventArgs
{
    public int Count { get; }

    public AllShimmeringToggledEventArgs(int count)
    {
        Count = count;
    }
}
