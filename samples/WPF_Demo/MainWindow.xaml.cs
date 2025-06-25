using Shimmer.Wpf;
using Shimmer.Wpf.Events;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace WPF_Demo;

public partial class MainWindow : Window
{
    private const string _groupName = "LeftGroup";
    public MainWindow()
    {
        DataContext = new MainViewModel();
        InitializeComponent();
        ShimmerControl.AllShimmeringStarted += ShimmerControl_AllShimmeringStarted;
        ShimmerControl.AllShimmeringStopped += ShimmerControl_AllShimmeringStopped;
        ShimmerControl.AllShimmeringToggled += ShimmerControl_AllShimmeringToggled;

        ShimmerControl.ShimmeringGroupStarted += ShimmerControl_ShimmeringGroupStarted;
        ShimmerControl.ShimmeringGroupStopped += ShimmerControl_ShimmeringGroupStopped;
        ShimmerControl.ShimmeringGroupToggled += ShimmerControl_ShimmeringGroupToggled;
    }


    private void ShimmerControl_AllShimmeringStarted(object? sender, AllShimmeringStartedEventArgs e)
    {
        PrintToConsole($"All shimmering started. Affected: {e.Count}");
    }

    private void ShimmerControl_AllShimmeringStopped(object? sender, AllShimmeringStoppedEventArgs e)
    {
        PrintToConsole($"All shimmering stopped. Affected: {e.Count}");
    }

    private void ShimmerControl_AllShimmeringToggled(object? sender, AllShimmeringToggledEventArgs e)
    {
        PrintToConsole($"All shimmering toggled. Affected: {e.Count}");
    }

    private void ShimmerControl_ShimmeringGroupStarted(object? sender, ShimmeringGroupStartedEventArgs e)
    {
        PrintToConsole($"Group '{e.GroupName}' shimmering started. Affected: {e.Count}");
    }

    private void ShimmerControl_ShimmeringGroupStopped(object? sender, ShimmeringGroupStoppedEventArgs e)
    {
        PrintToConsole($"Group '{e.GroupName}' shimmering stopped. Affected: {e.Count}");
    }

    private void ShimmerControl_ShimmeringGroupToggled(object? sender, ShimmeringGroupToggledEventArgs e)
    {
        PrintToConsole($"Group '{e.GroupName}' shimmering toggled. Affected: {e.Count}");
    }

    private void PrintToConsole(string text)
    {
        Debug.WriteLine(text);
        EventsListView.Items.Add(new TextBlock() { Text = text });

        EventsScrollViewer.Dispatcher.InvokeAsync(() =>
        {
            EventsScrollViewer.ScrollToEnd();
        }, System.Windows.Threading.DispatcherPriority.Background);
    }

    private void StartLeftGroupButton_Click(object sender, RoutedEventArgs e)
    {
        ShimmerControl.StartGroup(_groupName);
    }

    private void StopLeftGroupButton_Click(object sender, RoutedEventArgs e)
    {
        ShimmerControl.StopGroup(_groupName);
    }
    private void ToggleLeftGroupButton_Click(object sender, RoutedEventArgs e)
    {
        ShimmerControl.ToggleGroup(_groupName);
    }


    private void StartAllButton_Click(object sender, RoutedEventArgs e)
    {
        ShimmerControl.StartAll();
    }

    private void StopAllButton_Click(object sender, RoutedEventArgs e)
    {
        ShimmerControl.StopAll();
    }

    private void ToggleAllButton_Click(object sender, RoutedEventArgs e)
    {
        ShimmerControl.ToggleAll();
    }
}