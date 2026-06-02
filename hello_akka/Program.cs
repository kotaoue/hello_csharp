using Akka.Actor;

using var system = ActorSystem.Create("hello-akka");

var printer = system.ActorOf(Props.Create<PrinterActor>(), "printer");
var greeter = system.ActorOf(Props.Create(() => new GreeterActor(printer)), "greeter");
var askGreeter = system.ActorOf(Props.Create<AskGreeterActor>(), "ask-greeter");

greeter.Tell(new Greet("World"));
greeter.Tell(new Greet("Akka.NET"));

try
{
    var askReply = await askGreeter.Ask<Greeting>(new Greet("Ask Pattern"), TimeSpan.FromSeconds(1));
    Console.WriteLine($"Ask response: {askReply.Message}");
}
catch (AskTimeoutException)
{
    Console.WriteLine("Ask timeout: no response within 1 second.");
}

try
{
    var slowReply = await askGreeter.Ask<Greeting>(new SlowGreet("Slow Ask"), TimeSpan.FromMilliseconds(200));
    Console.WriteLine($"Slow ask response: {slowReply.Message}");
}
catch (AskTimeoutException)
{
    Console.WriteLine("Ask timeout (expected): slow actor did not respond within 200ms.");
}

await Task.Delay(100);
await system.Terminate();

record Greet(string Who);
record SlowGreet(string Who);
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

class AskGreeterActor : ReceiveActor
{
    public AskGreeterActor()
    {
        Receive<Greet>(msg => Sender.Tell(new Greeting($"Hello, {msg.Who}!")));
        Receive<SlowGreet>(msg =>
        {
            Thread.Sleep(1000);
            Sender.Tell(new Greeting($"Hello slowly, {msg.Who}!"));
        });
    }
}
