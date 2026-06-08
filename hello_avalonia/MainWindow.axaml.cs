using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace hello_avalonia;

public partial class MainWindow : Window
{
    public ObservableCollection<TodoItem> VisibleTodos { get; } = new();

    private int _count;
    private readonly ObservableCollection<TodoItem> _allTodos = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        UpdateCountText();
        UpdateVisibleTodos();
        UpdateSummaryText();
        UpdateThemeStatusText();
    }

    private void OnLightThemeClick(object? sender, RoutedEventArgs e)
    {
        ApplyTheme(ThemeVariant.Light);
    }

    private void OnDarkThemeClick(object? sender, RoutedEventArgs e)
    {
        ApplyTheme(ThemeVariant.Dark);
    }

    private void OnSystemThemeClick(object? sender, RoutedEventArgs e)
    {
        ApplyTheme(ThemeVariant.Default);
    }

    private void ApplyTheme(ThemeVariant variant)
    {
        if (Application.Current is null)
        {
            return;
        }

        Application.Current.RequestedThemeVariant = variant;
        UpdateThemeStatusText();
    }

    private void UpdateThemeStatusText()
    {
        if (Application.Current is null)
        {
            ThemeStatusText.Text = "現在: Unknown";
            return;
        }

        var label = Application.Current.RequestedThemeVariant switch
        {
            { Key: "Light" } => "Light",
            { Key: "Dark" } => "Dark",
            _ => "System"
        };

        ThemeStatusText.Text = $"現在: {label}";
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

    private void OnAddTodoClick(object? sender, RoutedEventArgs e)
    {
        var title = NewTodoTextBox.Text?.Trim();
        if (string.IsNullOrEmpty(title))
        {
            return;
        }

        var todo = new TodoItem { Title = title };
        todo.PropertyChanged += OnTodoPropertyChanged;

        _allTodos.Add(todo);
        NewTodoTextBox.Text = string.Empty;

        UpdateVisibleTodos();
        UpdateSummaryText();
    }

    private void OnDeleteTodoClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button deleteButton || deleteButton.DataContext is not TodoItem todo)
        {
            return;
        }

        todo.PropertyChanged -= OnTodoPropertyChanged;
        _allTodos.Remove(todo);
        UpdateVisibleTodos();
        UpdateSummaryText();
    }

    private void OnTodoDoneChanged(object? sender, RoutedEventArgs e)
    {
        UpdateVisibleTodos();
        UpdateSummaryText();
    }

    private void OnShowOnlyActiveChanged(object? sender, RoutedEventArgs e)
    {
        UpdateVisibleTodos();
    }

    private void OnTodoPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(TodoItem.IsDone))
        {
            return;
        }

        UpdateVisibleTodos();
        UpdateSummaryText();
    }

    private void UpdateVisibleTodos()
    {
        var showOnlyActive = ShowOnlyActiveCheckBox.IsChecked == true;

        VisibleTodos.Clear();
        foreach (var todo in _allTodos)
        {
            if (showOnlyActive && todo.IsDone)
            {
                continue;
            }

            VisibleTodos.Add(todo);
        }
    }

    private void UpdateSummaryText()
    {
        var remaining = 0;
        foreach (var todo in _allTodos)
        {
            if (!todo.IsDone)
            {
                remaining++;
            }
        }

        SummaryText.Text = $"合計: {_allTodos.Count}件 (未完了: {remaining}件)";
    }
}

public sealed class TodoItem : INotifyPropertyChanged
{
    private bool _isDone;

    public string Title { get; init; } = string.Empty;

    public bool IsDone
    {
        get => _isDone;
        set
        {
            if (_isDone == value)
            {
                return;
            }

            _isDone = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDone)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
