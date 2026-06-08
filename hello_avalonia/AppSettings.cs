using Avalonia;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace hello_avalonia;

public sealed class AppSettings
{
    public double WindowWidth { get; set; } = 560;
    public double WindowHeight { get; set; } = 420;
    public string Theme { get; set; } = ThemeVariant.Default.Key?.ToString() ?? "Default";
    public int CounterValue { get; set; }
    public string TodoInputText { get; set; } = string.Empty;
    public bool ShowOnlyActiveTodos { get; set; }
    public int SelectedTabIndex { get; set; }
    public List<TodoItemState> Todos { get; set; } = new();
}

public sealed class TodoItemState
{
    public string Title { get; set; } = string.Empty;
    public bool IsDone { get; set; }
}

public static class AppSettingsStore
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    public static string SettingsFilePath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "hello_avalonia",
        "settings.json");

    public static AppSettings Load()
    {
        try
        {
            if (!File.Exists(SettingsFilePath))
            {
                return new AppSettings();
            }

            var json = File.ReadAllText(SettingsFilePath);
            return JsonSerializer.Deserialize<AppSettings>(json, SerializerOptions) ?? new AppSettings();
        }
        catch
        {
            return new AppSettings();
        }
    }

    public static void Save(AppSettings settings)
    {
        var directoryPath = Path.GetDirectoryName(SettingsFilePath);
        if (!string.IsNullOrEmpty(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var json = JsonSerializer.Serialize(settings, SerializerOptions);
        File.WriteAllText(SettingsFilePath, json);
    }
}
