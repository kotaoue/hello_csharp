using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace hello_avalonia;

public partial class MainWindow : Window
{
    public ObservableCollection<TodoItem> VisibleTodos { get; } = new();

    private int _count;
    private readonly ObservableCollection<TodoItem> _allTodos = new();
    private readonly AppSettings _settings;

    public MainWindow(AppSettings settings)
    {
        _settings = settings;
        InitializeComponent();
        DataContext = this;
        Width = _settings.WindowWidth;
        Height = _settings.WindowHeight;
        RootTabs.SelectedIndex = Math.Max(0, _settings.SelectedTabIndex);
        _count = _settings.CounterValue;
        NewTodoTextBox.Text = _settings.TodoInputText;
        ShowOnlyActiveCheckBox.IsChecked = _settings.ShowOnlyActiveTodos;

        foreach (var todoState in _settings.Todos)
        {
            var todo = new TodoItem
            {
                Title = todoState.Title,
                IsDone = todoState.IsDone
            };
            todo.PropertyChanged += OnTodoPropertyChanged;
            _allTodos.Add(todo);
        }

        UpdateCountText();
        UpdateVisibleTodos();
        UpdateSummaryText();
        UpdateThemeStatusText();
        SaveSettings();

        Closing += OnClosing;
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
        SaveSettings();
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
        SaveSettings();
    }

    private void OnDecrementClick(object? sender, RoutedEventArgs e)
    {
        _count--;
        UpdateCountText();
        SaveSettings();
    }

    private void OnResetClick(object? sender, RoutedEventArgs e)
    {
        _count = 0;
        UpdateCountText();
        SaveSettings();
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
        SaveSettings();
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
        SaveSettings();
    }

    private void OnTodoDoneChanged(object? sender, RoutedEventArgs e)
    {
        UpdateVisibleTodos();
        UpdateSummaryText();
        SaveSettings();
    }

    private void OnShowOnlyActiveChanged(object? sender, RoutedEventArgs e)
    {
        UpdateVisibleTodos();
        SaveSettings();
    }

    private void OnTodoPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(TodoItem.IsDone))
        {
            return;
        }

        UpdateVisibleTodos();
        UpdateSummaryText();
        SaveSettings();
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

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        SaveSettings();
    }

    private void SaveSettings()
    {
        _settings.WindowWidth = Width;
        _settings.WindowHeight = Height;
        _settings.CounterValue = _count;
        _settings.TodoInputText = NewTodoTextBox.Text ?? string.Empty;
        _settings.ShowOnlyActiveTodos = ShowOnlyActiveCheckBox.IsChecked == true;
        _settings.SelectedTabIndex = RootTabs.SelectedIndex;
        _settings.Theme = Application.Current?.RequestedThemeVariant?.Key ?? ThemeVariant.Default.Key ?? "Default";
        _settings.Todos = _allTodos.Select(todo => new TodoItemState
        {
            Title = todo.Title,
            IsDone = todo.IsDone
        }).ToList();

        AppSettingsStore.Save(_settings);
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
