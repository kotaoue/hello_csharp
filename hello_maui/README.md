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

> **Note (Java SDK 警告):** `$JAVA_HOME` に Homebrew の `openjdk@17` が設定されている場合、`jvm` が見つからないという警告が出ることがある。
> ビルド自体は成功するが、解消するには `temurin` などのフル JDK をインストールして `JAVA_HOME` を切り替える。
>
> ```sh
> brew install --cask temurin@17
> export JAVA_HOME=/Library/Java/JavaVirtualMachines/temurin-17.jdk/Contents/Home
> ```

> **Note (`dotnet run` 実行時):** `dotnet run` には起動済みの Android エミュレーターまたは接続済みの実機が必要。
> `emulator` コマンドが見つからない場合は PATH を追加してから AVD を起動する。
>
> ```sh
> # PATH の追加（~/.zshrc に書いておくと永続化できる）
> export PATH="$PATH:$HOME/Library/Android/sdk/emulator"
> export PATH="$PATH:$HOME/Library/Android/sdk/platform-tools"
>
> # 利用可能な AVD を確認
> emulator -list-avds
>
> # エミュレーターを起動（AVD 名は上のコマンドで確認）
> emulator -avd <AVD_NAME> &
> emulator -avd Medium_Phone_API_35 &
>
> # 起動完了後に実行
> dotnet run -f net10.0-android
> ```
>
> AVD がまだ作成されていない場合は Android Studio の **Virtual Device Manager** で作成する。

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

> **Note (Xcode バージョン不一致):** インストール済みの .NET MacCatalyst ワークロードパックが古い場合、ビルド時に以下のようなエラーが発生することがある。
>
> ```
> error This version of .NET for MacCatalyst (x.x.xxxxx) requires Xcode x.x.
> The current version of Xcode is x.x.x.
> ```
>
> この場合はワークロードを最新化することで解消できる。
>
> ```sh
> sudo dotnet workload update
> ```

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
