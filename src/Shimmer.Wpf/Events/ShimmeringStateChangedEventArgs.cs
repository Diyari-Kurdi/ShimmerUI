namespace Shimmer.Wpf.Events;

public class ShimmeringStateChangedEventArgs : EventArgs
{
    public bool IsShimmering { get; }

    public ShimmeringStateChangedEventArgs(bool isShimmering)
    {
        IsShimmering = isShimmering;
    }
}