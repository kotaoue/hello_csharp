using Avalonia.Controls;
using Avalonia.Interactivity;

namespace hello_avalonia;

public partial class MainWindow : Window
{
    private int _count;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnIncrementClick(object? sender, RoutedEventArgs e)
    {
        _count++;
        UpdateCountText();
    }

    private void OnDecrementClick(object? sender, RoutedEventArgs e)
    {
        _count--;
        UpdateCountText();
    }

    private void OnResetClick(object? sender, RoutedEventArgs e)
    {
        _count = 0;
        UpdateCountText();
    }

    private void UpdateCountText()
    {
        CountText.Text = $"Count: {_count}";
    }
}