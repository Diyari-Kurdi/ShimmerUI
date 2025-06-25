![ShimmerWPF_Icon_256](https://github.com/user-attachments/assets/644ff578-2eaa-416e-92ef-206f5ff0e432)

# Shimmer.Wpf

**Shimmer.Wpf** is a customizable WPF control that displays a shimmer loading effect to enhance the user experience during data loading.

![Light](https://github.com/user-attachments/assets/9bc9c224-6539-4294-b459-731de6fc8585)  ![Dark](https://github.com/user-attachments/assets/d2f05eff-5e13-4397-b37f-ecbc5f865af8)


## ✨ Features

- ⚡ **Fast & Lightweight** – Optimized for smooth WPF performance.
- 🎨 **Fully Customizable** – Shimmer color, speed, width, opacity, duration, and corner radius.
- 🔄 **MVVM-Friendly** – `IsShimmering` is a bindable dependency property.
- 🚀 **Auto-Start Support** – Automatically starts shimmer on load if enabled.
- 🖥️ **Design-Time Support** – Preview shimmer directly in XAML designer.
- 🧩 **Group Control** – Manage grouped shimmer controls via `StartGroup`, `StopGroup`, and `ToggleGroup`.
- 🌐 **Global Control** – Control all shimmer instances using static methods: `StartAll`, `StopAll`, `ToggleAll`.
- 📡 **Event Hooks** – React to shimmer lifecycle events at instance, group, or global level.


## 📦 Installation

```sh
Install-Package Shimmer.Wpf
```

or with .NET CLI:

```sh
dotnet add package Shimmer.Wpf
```

### 🚀 Example Usage

#### ✅ **1. Content with its own CornerRadius**

```xml
<shimmer:ShimmerControl AutoStart="True">
    <Border Width="150"
            Height="150"
            CornerRadius="20"
            Background="#444">
        <TextBlock Text="Rounded Content"
                   HorizontalAlignment="Center"
                   VerticalAlignment="Center"
                   Foreground="White"/>
    </Border>
</shimmer:ShimmerControl>
```

> ✅ The control detects `CornerRadius` from the content if present.

---

#### ✅ **2. ShimmerControl with CornerRadius**

```xml
<shimmer:ShimmerControl
    Width="200"
    Height="30"
    AutoStart="True"
    CornerRadius="15"
    ShimmerDuration="0:0:0.8">
    <TextBlock
        HorizontalAlignment="Center"
        VerticalAlignment="Center"
        FontSize="24"
        Text="Hello World" />
</shimmer:ShimmerControl>
```

> ✅ Use this when your content (e.g., `TextBlock`) doesn’t support `CornerRadius`.

---

#### ✅ **3. Compact placeholder bars (often for skeleton loaders)**

```xml
<StackPanel>
    <shimmer:ShimmerControl
        Margin="0,0,0,10"
        AutoStart="True"
        DesignModeShimmering="True"
        GroupName="LoadingCards">
        <Border
            Width="200"
            Height="200"
            Background="Beige"
            CornerRadius="100" />
    </shimmer:ShimmerControl>
    <shimmer:ShimmerControl
        Width="125"
        Height="10"
        Margin="0,0,0,5"
        HorizontalAlignment="Center"
        AutoStart="True"
        CornerRadius="6"
        GroupName="LoadingCards" />
    <shimmer:ShimmerControl
        Width="80"
        Height="10"
        HorizontalAlignment="Center"
        AutoStart="True"
        CornerRadius="6"
        GroupName="LoadingCards" />
</StackPanel>
```

---

## 🧩 Usage

### XAML Binding

Bind the shimmer state to your view model:

```xml
<shimmer:ShimmerControl IsShimmering="{Binding IsLoading}" />
```

### Automatic Start

Use `AutoStart` to shimmer immediately when the control loads:

```xml
<shimmer:ShimmerControl AutoStart="True" />
```

### Controlling Shimmer Programmatically

You can control shimmer effects globally, by group, or individually using static methods:

- **StartAll()** — Starts shimmer on all existing `ShimmerControl` instances.

```csharp
ShimmerControl.StartAll();
```

- **StopAll()** — Stops shimmer on all instances.

```csharp
ShimmerControl.StopAll();
```

- **ToggleAll()** — Toggles shimmer state on all instances.

```csharp
ShimmerControl.ToggleAll();
```

- **StartGroup(string groupName)** — Starts shimmer on all controls in the specified group.

```csharp
ShimmerControl.StartGroup("LoadingCards");
```

- **StopGroup(string groupName)** — Stops shimmer on all controls in the specified group.

```csharp
ShimmerControl.StopGroup("LoadingCards");
```

- **ToggleGroup(string groupName)** — Toggles shimmer on all controls in the specified group.

```csharp
ShimmerControl.ToggleGroup("LoadingCards");
```

### Individual Control Methods

You can also start, stop or toggle shimmer on individual controls:

```csharp
myShimmerControl.StartShimmering();
myShimmerControl.StopShimmering();
myShimmerControl.ToggleShimmering();
```

### Events

Subscribe to static events to react to shimmer changes globally or per group:

```csharp
ShimmerControl.ShimmeringGroupStarted += (s, e) => {
    string group = e.GroupName;
    int count = e.Count;
    // handle group started
};

ShimmerControl.ShimmeringGroupStopped += (s, e) => {
    string group = e.GroupName;
    int count = e.Count;
    // handle group stopped
};

ShimmerControl.ShimmeringGroupToggled += (s, e) => {
    string group = e.GroupName;
    int count = e.Count;
    // handle group toggled
};

ShimmerControl.AllShimmeringStarted += (s, e) => {
    int count = e.Count;
    // handle all shimmer started
};

ShimmerControl.AllShimmeringStopped += (s, e) => {
    int count = e.Count;
    // handle all shimmer stopped
};

ShimmerControl.AllShimmeringToggled += (s, e) => {
    int count = e.Count;
    // handle all shimmer toggled
};
```


## 🎥 Preview
The recording is from the included `WPF_Demo` project, showcasing various usages of the control.

https://github.com/user-attachments/assets/a18bda0a-75df-4751-952e-8366c51554a4

## 🛠 Properties

| Property                 | Description                                  |
|--------------------------|----------------------------------------------|
| `IsShimmering`           | Starts or stops the shimmer animation        |
| `AutoStart`              | Starts shimmering automatically on load      |
| `ShimmerColor`           | Highlight color of the shimmer               |
| `ShimmerWidth`           | Width of the shimmer band                    |
| `ShimmerOpacity`         | Opacity of the shimmer effect                |
| `ShimmerDuration`        | Total animation cycle duration               |
| `CornerRadius`           | Rounding of shimmer border                   |
| `ShimmerGroup`           | Optional group identifier for group control  |
| `ShimmerEasingFunction`  | Easing function for animation                |
| `DesignModeShimmering`   | Enables shimmer effect in design view        |

## 📄 License

This project is licensed under the [MIT License](https://github.com/Diyari-Kurdi/ShimmerUI/blob/master/LICENSE.txt).  
© 2025 Diyari Ismael
