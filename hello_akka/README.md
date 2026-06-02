# Hello World (Akka.NET)

A simple Hello World application using [Akka.NET](https://getakka.net/), the actor-based concurrency framework for .NET.

## Features

- **Akka.NET** — Actor model framework for building concurrent and distributed systems on .NET
- **Actor-based messaging** — Demonstrates the core actor pattern: define actors, send messages, and handle responses
- **GreeterActor** — Receives a `Greet` message and forwards a `Greeting` to the printer
- **PrinterActor** — Receives a `Greeting` message and prints it to the console

## Requirements

- [.NET SDK](https://dotnet.microsoft.com/download)

## Build & Run

```sh
cd hello_akka
dotnet run
```

Expected output:

```text
Hello, World!
Hello, Akka.NET!
```
