using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;
using System.Globalization;

namespace hello_avalonia;

public partial class SettingsWindow : Window
{
    private readonly AppSettings _settings;
    private readonly MainWindow _ownerWindow;

    public SettingsWindow(AppSettings settings, MainWindow ownerWindow)
    {
        _settings = settings;
        _ownerWindow = ownerWindow;

        InitializeComponent();

        ThemeComboBox.ItemsSource = new[] { "System", "Light", "Dark" };
        ThemeComboBox.SelectedItem = NormalizeThemeName(_settings.Theme);
        WindowWidthTextBox.Text = _settings.WindowWidth.ToString(CultureInfo.InvariantCulture);
        WindowHeightTextBox.Text = _settings.WindowHeight.ToString(CultureInfo.InvariantCulture);
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void OnSaveClick(object? sender, RoutedEventArgs e)
    {
        if (!double.TryParse(WindowWidthTextBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var width) ||
            !double.TryParse(WindowHeightTextBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var height))
        {
            StatusTextBlock.Text = "幅と高さは数値で入力してください。";
            return;
        }

        var themeName = ThemeComboBox.SelectedItem?.ToString() ?? "System";

        _settings.WindowWidth = width;
        _settings.WindowHeight = height;
        _settings.Theme = themeName;
        AppSettingsStore.Save(_settings);

        ApplyTheme(themeName);
        _ownerWindow.Width = width;
        _ownerWindow.Height = height;
        _ownerWindow.RefreshThemeStatusText();

        Close();
    }

    private static string NormalizeThemeName(string theme)
    {
        return theme switch
        {
            "Light" => "Light",
            "Dark" => "Dark",
            _ => "System"
        };
    }

    private static void ApplyTheme(string themeName)
    {
        if (Application.Current is null)
        {
            return;
        }

        Application.Current.RequestedThemeVariant = themeName switch
        {
            "Light" => ThemeVariant.Light,
            "Dark" => ThemeVariant.Dark,
            _ => ThemeVariant.Default
        };
    }
}
