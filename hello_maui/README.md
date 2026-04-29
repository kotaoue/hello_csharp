# Hello World (.NET MAUI)

A simple Hello World application using .NET MAUI (Multi-platform App UI).

## Features

- **.NET MAUI** — Cross-platform native UI framework for Android, iOS, macOS, and Windows
- **XAML UI** — Declarative UI layout using XAML markup
- **C# code-behind** — Event-driven logic in C#
- **Cross-platform** — Single codebase targets Android, iOS, macOS, and Windows

## Requirements

- [.NET SDK](https://dotnet.microsoft.com/download) (10.0 or later)
- Platform-specific workload:

| Target | Required workload / tool |
| --- | --- |
| Android | `dotnet workload install maui-android` + Android SDK |
| iOS | `dotnet workload install maui-ios` + Xcode (macOS only) |
| macOS | `dotnet workload install maui-maccatalyst` + Xcode |
| Windows | `dotnet workload install maui-windows` (Windows only) |

```sh
sudo dotnet workload install maui-android
sudo dotnet workload install maui-ios
sudo dotnet workload install maui-maccatalyst
sudo dotnet workload install maui-windows
```

## Build & Run

### Android

```sh
cd hello_maui
dotnet build -f net10.0-android
dotnet run -f net10.0-android
```

### iOS (macOS only)

```sh
cd hello_maui
dotnet build -f net10.0-ios
dotnet run -f net10.0-ios
```

### macOS (macOS only)

```sh
cd hello_maui
dotnet build -f net10.0-maccatalyst
dotnet run -f net10.0-maccatalyst
```

### Windows (Windows only)

```sh
cd hello_maui
dotnet build -f net10.0-windows10.0.19041.0
dotnet run -f net10.0-windows10.0.19041.0
```

## Install Workloads

Install the required MAUI workload for your target platform before building:

```sh
# Android
dotnet workload install maui-android

# iOS / macOS (run on macOS)
dotnet workload install maui-ios
dotnet workload install maui-maccatalyst

# Windows (run on Windows)
dotnet workload install maui-windows
```
