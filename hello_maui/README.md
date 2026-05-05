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

> **Note (Xcode バージョン不一致):** インストール済みの .NET MacCatalyst ワークロードパックが古い場合、ビルド時に以下のようなエラーが発生することがある。
>
> ```text
> error This version of .NET for MacCatalyst (x.x.xxxxx) requires Xcode x.x.
> The current version of Xcode is x.x.x.
> ```
>
> この場合はワークロードを最新化することで解消できる。
>
> ```sh
> sudo dotnet workload update
> ```
