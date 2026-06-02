using Akka.Actor;

using var system = ActorSystem.Create("hello-akka");

var printer = system.ActorOf(Props.Create<PrinterActor>(), "printer");
var greeter = system.ActorOf(Props.Create(() => new GreeterActor(printer)), "greeter");

greeter.Tell(new Greet("World"));
greeter.Tell(new Greet("Akka.NET"));

await Task.Delay(100);
await system.Terminate();

record Greet(string Who);
record Greeting(string Message);

class GreeterActor : ReceiveActor
{
    public GreeterActor(IActorRef printer)
    {
        Receive<Greet>(msg => printer.Tell(new Greeting($"Hello, {msg.Who}!")));
    }
}

class PrinterActor : ReceiveActor
{
    public PrinterActor()
    {
        Receive<Greeting>(msg => Console.WriteLine(msg.Message));
    }
}
