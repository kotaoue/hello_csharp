# Hello World (ASP.NET Core)

A simple Hello World web application using ASP.NET Core.

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

## Stop

Press `Ctrl+C` in the terminal to stop the server.
