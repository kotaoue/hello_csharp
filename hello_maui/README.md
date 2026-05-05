# Hello World (.NET MAUI)

A simple Hello World application using .NET MAUI (Multi-platform App UI) for macOS.

## Features

- **.NET MAUI** — Native UI framework for macOS
- **XAML UI** — Declarative UI layout using XAML markup
- **C# code-behind** — Event-driven logic in C#

## Requirements

- [.NET SDK](https://dotnet.microsoft.com/download) (10.0 or later)
- Xcode
- `maui-maccatalyst` workload:

```sh
sudo dotnet workload install maui-maccatalyst
```

## Build & Run

```sh
cd hello_maui
dotnet build -f net10.0-maccatalyst
dotnet run -f net10.0-maccatalyst
```
