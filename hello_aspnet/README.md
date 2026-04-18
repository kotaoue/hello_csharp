# Hello World (ASP.NET Core)

A simple Hello World web application using ASP.NET Core.

## Requirements

- [.NET SDK](https://dotnet.microsoft.com/download)

## Build & Run

```sh
cd hello_aspnet
dotnet run
```

If the build succeeds, check <http://localhost:5000/>.

## Endpoints

- `GET /` returns `Hello, World!`
- `GET /greet/{name}` returns `Hello, {name}!`

You can test with curl:

```sh
curl http://localhost:5000/
curl http://localhost:5000/greet/Alice
```

## Stop

Press `Ctrl+C` in the terminal to stop the server.
