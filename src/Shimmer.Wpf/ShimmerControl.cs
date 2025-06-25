using Shimmer.Wpf.Events;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Shimmer.Wpf;

public class ShimmerControl : ContentControl
{
    private static readonly HashSet<ShimmerControl> _instances = [];

    public static event EventHandler<AllShimmeringStartedEventArgs>? AllShimmeringStarted;
    public static event EventHandler<AllShimmeringStoppedEventArgs>? AllShimmeringStopped;
    public static event EventHandler<AllShimmeringToggledEventArgs>? AllShimmeringToggled;

    public static event EventHandler<ShimmeringGroupStartedEventArgs>? ShimmeringGroupStarted;
    public static event EventHandler<ShimmeringGroupStoppedEventArgs>? ShimmeringGroupStopped;
    public static event EventHandler<ShimmeringGroupToggledEventArgs>? ShimmeringGroupToggled;


    public event EventHandler? ShimmeringStarted;
    public event EventHandler? ShimmeringStopped;
    public event EventHandler? ShimmeringToggled;
    public event EventHandler<ShimmeringStateChangedEventArgs>? ShimmeringStateChanged;

    static ShimmerControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ShimmerControl), new FrameworkPropertyMetadata(typeof(ShimmerControl)));
    }

    public ShimmerControl()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DesignerProperties.GetIsInDesignMode(this))
        {
            SetCurrentValue(IsShimmeringProperty, DesignModeShimmering);
            return;
        }

        _instances.Add(this);
        if (AutoStart && !IsShimmering)
        {
            IsShimmering = true;
        }
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        _instances.Remove(this);
    }

    #region Dependency Properties

    public bool DesignModeShimmering { get; set; }

    public static readonly DependencyProperty IsShimmeringProperty =
        DependencyProperty.Register(nameof(IsShimmering), typeof(bool), typeof(ShimmerControl),
            new PropertyMetadata(false, OnIsShimmeringChanged));

    public bool IsShimmering
    {
        get => (bool)GetValue(IsShimmeringProperty);
        set => SetValue(IsShimmeringProperty, value);
    }

    public static readonly DependencyProperty AutoStartProperty =
    DependencyProperty.Register(nameof(AutoStart), typeof(bool), typeof(ShimmerControl),
        new PropertyMetadata(false));

    public bool AutoStart
    {
        get => (bool)GetValue(AutoStartProperty);
        set => SetValue(AutoStartProperty, value);
    }

    public static readonly DependencyProperty ShimmerColorProperty =
        DependencyProperty.Register(nameof(ShimmerColor), typeof(Color), typeof(ShimmerControl),
            new PropertyMetadata(Colors.White, OnShimmerPropertyChanged));

    public Color ShimmerColor
    {
        get => (Color)GetValue(ShimmerColorProperty);
        set => SetValue(ShimmerColorProperty, value);
    }

    public static readonly DependencyProperty ShimmerWidthProperty =
        DependencyProperty.Register(nameof(ShimmerWidth), typeof(double), typeof(ShimmerControl),
            new PropertyMetadata(80.0));

    public double ShimmerWidth
    {
        get => (double)GetValue(ShimmerWidthProperty);
        set => SetValue(ShimmerWidthProperty, value);
    }

    public static readonly DependencyProperty ShimmerOpacityProperty =
        DependencyProperty.Register(nameof(ShimmerOpacity), typeof(double), typeof(ShimmerControl),
            new PropertyMetadata(0.6));

    public double ShimmerOpacity
    {
        get => (double)GetValue(ShimmerOpacityProperty);
        set => SetValue(ShimmerOpacityProperty, value);
    }

    public static readonly DependencyProperty ShimmerDurationProperty =
        DependencyProperty.Register(nameof(ShimmerDuration), typeof(Duration), typeof(ShimmerControl),
            new PropertyMetadata(new Duration(TimeSpan.FromSeconds(1)), OnShimmerPropertyChanged));

    public Duration ShimmerDuration
    {
        get => (Duration)GetValue(ShimmerDurationProperty);
        set => SetValue(ShimmerDurationProperty, value);
    }

    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ShimmerControl),
            new PropertyMetadata(new CornerRadius(5)));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly DependencyProperty ShimmerEasingFunctionProperty =
        DependencyProperty.Register(nameof(ShimmerEasingFunction), typeof(IEasingFunction), typeof(ShimmerControl),
            new PropertyMetadata(null, OnShimmerPropertyChanged));

    public IEasingFunction? ShimmerEasingFunction
    {
        get => (IEasingFunction?)GetValue(ShimmerEasingFunctionProperty);
        set => SetValue(ShimmerEasingFunctionProperty, value);
    }

    public string? GroupName
    {
        get => (string?)GetValue(GroupNameProperty);
        set => SetValue(GroupNameProperty, value);
    }

    public static readonly DependencyProperty GroupNameProperty =
        DependencyProperty.Register(nameof(GroupName), typeof(string), typeof(ShimmerControl), new PropertyMetadata(null));

    #endregion

    #region Property Changed Callbacks

    private static void OnIsShimmeringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ShimmerControl control)
            return;
        bool isNow = (bool)e.NewValue;

        VisualStateManager.GoToState(control, isNow ? "Shimmering" : "Idle", true);
        control.UpdateVisibility();

        if (isNow)
            control.ShimmeringStarted?.Invoke(control, EventArgs.Empty);
        else
            control.ShimmeringStopped?.Invoke(control, EventArgs.Empty);

        control.ShimmeringStateChanged?.Invoke(control, new ShimmeringStateChangedEventArgs(isNow));
    }

    private static void OnShimmerPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ShimmerControl control && control.IsLoaded)
        {
            control.OnApplyTemplate();
        }
    }

    #endregion

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        ApplyShimmerAnimation();
        ApplyShimmerVisuals();
    }

    private void ApplyShimmerAnimation()
    {
        if (GetTemplateChild("ShimmerAnimation") is DoubleAnimation shimmerAnim)
        {
            shimmerAnim.Duration = ShimmerDuration;
            shimmerAnim.EasingFunction = ShimmerEasingFunction;

            if (GetTemplateChild("ShimmerBorder") is Border shimmerBorder)
            {
                shimmerBorder.SizeChanged -= ShimmerBorder_SizeChanged;
                shimmerBorder.SizeChanged += ShimmerBorder_SizeChanged;
                ShimmerBorder_SizeChanged(shimmerBorder, null);
            }
        }
    }

    private void ShimmerBorder_SizeChanged(object? sender, SizeChangedEventArgs? e)
    {
        if (sender is Border shimmerBorder && GetTemplateChild("ShimmerAnimation") is DoubleAnimation shimmerAnim)
        {
            shimmerAnim.From = -shimmerBorder.ActualWidth;
            shimmerAnim.To = shimmerBorder.ActualWidth;
            shimmerBorder.CornerRadius = ResolveCornerRadius();
        }
    }

    /// <summary>
    /// Resolves the effective <see cref="CornerRadius"/> for the control.
    /// </summary>
    /// <remarks>This method attempts to retrieve the <see cref="CornerRadius"/> from the content of the
    /// control if it is a <see cref="FrameworkElement"/> with a property named "CornerRadius". If no such property is
    /// found or the content does not meet the required conditions, the control's own <see cref="CornerRadius"/> is
    /// returned.</remarks>
    /// <returns>The resolved <see cref="CornerRadius"/> value, either from the content's "CornerRadius" property or the
    /// control's own <see cref="CornerRadius"/>.</returns>
    private CornerRadius ResolveCornerRadius()
    {
        if (GetTemplateChild("PART_Content") is ContentPresenter cp && cp.Content is FrameworkElement fe)
        {
            PropertyInfo? prop = fe.GetType().GetProperty("CornerRadius");
            if (prop?.PropertyType == typeof(CornerRadius) && prop.GetValue(fe) is CornerRadius cr)
                return cr;
        }
        return CornerRadius;
    }

    private void ApplyShimmerVisuals()
    {
        if (GetTemplateChild("ShimmerRectangle") is Rectangle shimmerRect)
        {
            shimmerRect.Fill = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0),
                GradientStops =
                    {
                        new GradientStop(Colors.Transparent, 0.0),
                        new GradientStop(ShimmerColor, 0.45),
                        new GradientStop(ShimmerColor, 0.5),
                        new GradientStop(ShimmerColor, 0.55),
                        new GradientStop(Colors.Transparent, 1.0)
                    }
            };
        }
    }

    private void UpdateVisibility()
    {
        if (GetTemplateChild("ShimmerBorder") is Grid rootGrid)
        {
            rootGrid.Opacity = IsShimmering ? 0 : 1;
            rootGrid.Visibility = IsShimmering ? Visibility.Visible : Visibility.Collapsed;
        }

        if (GetTemplateChild("PART_Content") is ContentPresenter content)
            content.Opacity = IsShimmering ? 0 : 1;
    }

    public void StartShimmering()
    {
        IsShimmering = true;
    }
    public void StopShimmering()
    {
        IsShimmering = false;
    }
    public void ToggleShimmering()
    {
        IsShimmering = !IsShimmering;
    }

    /// <summary>
    /// Stops all shimmering effects on the registered controls.
    /// </summary>
    /// <remarks>This method iterates through all registered controls and disables the shimmering effect for
    /// those that are currently shimmering. If any controls are affected, the  <see cref="AllShimmeringStopped"/> event
    /// is raised with the count of affected controls.</remarks>
    public static void StopAll()
    {
        int affected = 0;
        foreach (var control in _instances)
        {
            if (control.IsShimmering)
            {
                SetIsShimmeringSafe(control, false);
                affected++;
            }
        }

        if (affected > 0)
        {
            AllShimmeringStopped?.Invoke(null, new AllShimmeringStoppedEventArgs(affected));
        }
    }

    /// <summary>
    /// Starts the shimmering effect for all controls that are not currently shimmering.
    /// </summary>
    /// <remarks>This method iterates through all registered controls and enables the shimmering effect  for
    /// those that are not already shimmering. If any controls are affected, the  <see cref="AllShimmeringStarted"/>
    /// event is raised with the count of affected controls.</remarks>
    public static void StartAll()
    {
        int affected = 0;
        foreach (var control in _instances)
        {
            if (!control.IsShimmering)
            {
                SetIsShimmeringSafe(control, true);
                affected++;
            }
        }

        if (affected > 0)
        {
            AllShimmeringStarted?.Invoke(null, new AllShimmeringStartedEventArgs(affected));
        }
    }

    /// <summary>
    /// Toggles the shimmering state of all registered controls.
    /// </summary>
    /// <remarks>This method iterates through all registered controls and reverses their current shimmering
    /// state. If a control is shimmering, it will be turned off; if it is not shimmering, it will be turned on. After
    /// toggling, the <see cref="AllShimmeringToggled"/> event is raised with the count of controls that were toggled on
    /// and off.</remarks>
    public static void ToggleAll()
    {
        int toggled =0;

        foreach (var control in _instances)
        {
            SetIsShimmeringSafe(control, !control.IsShimmering);
            toggled++;
        }

        if (toggled > 0)
        {
            AllShimmeringToggled?.Invoke(null, new AllShimmeringToggledEventArgs(toggled));
        }
    }

    /// <summary>
    /// Starts shimmering for all controls in the specified group.
    /// </summary>
    /// <remarks>This method iterates through all controls associated with the specified group and enables
    /// shimmering for those that are not already shimmering. If any controls are affected, the <see
    /// cref="ShimmeringGroupStarted"/> event is raised to notify listeners.</remarks>
    /// <param name="groupName">The name of the group whose controls should start shimmering. Cannot be null or empty.</param>
    public static void StartGroup(string groupName)
    {
        int affected = 0;

        foreach (var control in _instances.Where(c => c.GroupName == groupName))
        {
            if (!control.IsShimmering)
            {
                SetIsShimmeringSafe(control, true);
                affected++;
            }
        }

        if (affected > 0)
        {
            ShimmeringGroupStarted?.Invoke(null, new ShimmeringGroupStartedEventArgs(groupName, affected));
        }
    }

    /// <summary>
    /// Stops the shimmering effect for all controls in the specified group.
    /// </summary>
    /// <remarks>This method iterates through all controls associated with the specified group and disables
    /// the shimmering effect for any control that is currently shimmering. If any controls are affected, the <see
    /// cref="ShimmeringGroupStopped"/>  event is raised to notify listeners.</remarks>
    /// <param name="groupName">The name of the group whose shimmering effect should be stopped. Cannot be null or empty.</param>
    public static void StopGroup(string groupName)
    {
        int affected = 0;

        foreach (var control in _instances.Where(c => c.GroupName == groupName))
        {
            if (control.IsShimmering)
            {
                SetIsShimmeringSafe(control, false);
                affected++;
            }
        }

        if (affected > 0)
        {
            ShimmeringGroupStopped?.Invoke(null, new ShimmeringGroupStoppedEventArgs(groupName, affected));
        }
    }

    /// <summary>
    /// Toggles the shimmering state of all controls within the specified group.
    /// </summary>
    /// <remarks>This method inverts the shimmering state of each control in the specified group. If any
    /// controls are toggled, the <see cref="ShimmeringGroupToggled"/> event is raised, providing details about the
    /// number of controls toggled on and off.</remarks>
    /// <param name="groupName">The name of the group whose controls should have their shimmering state toggled.</param>
    public static void ToggleGroup(string groupName)
    {
        var controls = _instances.Where(c => c.GroupName == groupName).ToList();
        int count = 0;

        foreach (var control in controls)
        {
            SetIsShimmeringSafe(control, !control.IsShimmering);
                count++;
        }

        if (count > 0)
        {
            ShimmeringGroupToggled?.Invoke(null, new ShimmeringGroupToggledEventArgs(groupName, count));
        }
    }

    /// <summary>
    /// Sets the shimmering state of the specified <see cref="ShimmerControl"/> in a safe manner, either by updating the
    /// bound data context property or directly setting the control's property.
    /// </summary>
    /// <remarks>If the <paramref name="control"/> has a binding on the <c>IsShimmering</c> property, this
    /// method attempts to update the bound property in the data context. If no binding exists or the binding cannot be
    /// resolved, the <c>IsShimmering</c> property of the control is set directly.</remarks>
    /// <param name="control">The <see cref="ShimmerControl"/> whose shimmering state is to be updated.</param>
    /// <param name="value">A <see langword="true"/> to enable shimmering; otherwise, <see langword="false"/>.</param>
    private static void SetIsShimmeringSafe(ShimmerControl control, bool value)
    {
        var binding = BindingOperations.GetBindingExpression(control, IsShimmeringProperty);

        if (binding?.ParentBinding.Path?.Path is string path &&
            binding.DataItem is object dataContext)
        {
            var prop = dataContext.GetType().GetProperty(path);
            prop?.SetValue(dataContext, value);
        }
        else
        {
            control.IsShimmering = value;
        }
    }
}