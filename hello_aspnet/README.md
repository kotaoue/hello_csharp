# Hello World (ASP.NET Core)

A simple Hello World web application using ASP.NET Core.

## Features

- **ASP.NET Core Minimal API** — Lightweight HTTP endpoints with minimal boilerplate
- **Cross-platform** — Runs on Linux, macOS, and Windows via .NET
- **Routing** — Supports parameterised routes (e.g. `/greet/{name}`, `/add/{a}/{b}`)
- **Dynamic HTML** — Generates HTML responses at runtime using C# string interpolation
- **Input validation** — Validates colour values before rendering to prevent unexpected output

## Requirements

- [.NET SDK](https://dotnet.microsoft.com/download)

## Build & Run

```sh
cd hello_aspnet
dotnet run
```

If the build succeeds, check:

- [GET /](http://localhost:5000/) returns Hello, World!
- [GET /greet/{name}](http://localhost:5000/greet/Alice) returns Hello, {name}!
- [GET /add/{a:int}/{b:int}](http://localhost:5000/add/2/3) returns {a} + {b} = {a + b} (example: 2 + 3 = 5)
- [GET /bg/{color}](http://localhost:5000/bg/skyblue) shows a page with the specified background color
- [GET /bg/{color} (hex example)](http://localhost:5000/bg/%2300ff88)

## Stop

Press `Ctrl+C` in the terminal to stop the server.
