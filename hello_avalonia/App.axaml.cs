using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace hello_avalonia;

public partial class App : Application
{
    public AppSettings Settings { get; private set; } = new();

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        Settings = AppSettingsStore.Load();
        RequestedThemeVariant = ParseTheme(Settings.Theme);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow(Settings);
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static ThemeVariant ParseTheme(string theme)
    {
        return theme switch
        {
            "Light" => ThemeVariant.Light,
            "Dark" => ThemeVariant.Dark,
            _ => ThemeVariant.Default
        };
    }
}
