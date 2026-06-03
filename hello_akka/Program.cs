using Akka.Actor;
using Akka.Routing;

using var system = ActorSystem.Create("hello-akka");

var printer = system.ActorOf(Props.Create<PrinterActor>(), "printer");
var greeter = system.ActorOf(Props.Create(() => new GreeterActor(printer)), "greeter");
var askGreeter = system.ActorOf(Props.Create<AskGreeterActor>(), "ask-greeter");
var workerRouter = system.ActorOf(new RoundRobinPool(3).Props(Props.Create<WorkerActor>()), "worker-router");
var supervisorDemo = system.ActorOf(Props.Create<SupervisorDemoActor>(), "supervisor-demo");
var schedulerDemo = system.ActorOf(Props.Create<SchedulerDemoActor>(), "scheduler-demo");

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

for (var i = 1; i <= 10; i++)
{
    workerRouter.Tell(new Work(i));
}

supervisorDemo.Tell(new Increment());
supervisorDemo.Tell(new Increment());
supervisorDemo.Tell(new PrintState("before failures"));
supervisorDemo.Tell(new CrashResume());
supervisorDemo.Tell(new PrintState("after resume"));
supervisorDemo.Tell(new CrashRestart());
supervisorDemo.Tell(new PrintState("after restart"));

schedulerDemo.Tell(StartTicker.Instance);

await Task.Delay(6000);
await system.Terminate();

record Greet(string Who);
record SlowGreet(string Who);
record Greeting(string Message);
record Work(int Id);
record Increment;
record PrintState(string Label);
record CrashRestart;
record CrashResume;
sealed record StartTicker
{
    public static StartTicker Instance { get; } = new();
    private StartTicker() { }
}
sealed record Tick
{
    public static Tick Instance { get; } = new();
    private Tick() { }
}

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

class WorkerActor : ReceiveActor
{
    public WorkerActor()
    {
        Receive<Work>(msg =>
            Console.WriteLine($"Worker {Self.Path.Name} handled job #{msg.Id} on thread {Environment.CurrentManagedThreadId}"));
    }
}

class SupervisorDemoActor : ReceiveActor
{
    private readonly IActorRef _counter;

    public SupervisorDemoActor()
    {
        _counter = Context.ActorOf(Props.Create<CounterActor>(), "counter");

        Receive<Increment>(msg => _counter.Forward(msg));
        Receive<PrintState>(msg => _counter.Forward(msg));
        Receive<CrashRestart>(msg => _counter.Forward(msg));
        Receive<CrashResume>(msg => _counter.Forward(msg));
    }

    protected override SupervisorStrategy SupervisorStrategy()
    {
        return new OneForOneStrategy(
            maxNrOfRetries: 3,
            withinTimeRange: TimeSpan.FromMinutes(1),
            localOnlyDecider: ex => ex switch
            {
                InvalidOperationException => Directive.Restart,
                ArithmeticException => Directive.Resume,
                _ => Directive.Stop
            });
    }
}

class CounterActor : ReceiveActor
{
    private int _count;

    public CounterActor()
    {
        Receive<Increment>(_ => _count++);
        Receive<PrintState>(msg => Console.WriteLine($"Counter state [{msg.Label}]: {_count}"));
        Receive<CrashRestart>(_ => throw new InvalidOperationException("restart demo"));
        Receive<CrashResume>(_ => throw new ArithmeticException("resume demo"));
    }

    protected override void PreRestart(Exception reason, object message)
    {
        Console.WriteLine($"Counter restart triggered by: {reason.GetType().Name}");
        base.PreRestart(reason, message);
    }
}

class SchedulerDemoActor : ReceiveActor
{
    private int _ticks;
    private ICancelable? _cancelable;

    public SchedulerDemoActor()
    {
        Receive<StartTicker>(_ =>
        {
            _cancelable ??= Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(
                initialDelay: TimeSpan.Zero,
                interval: TimeSpan.FromSeconds(1),
                receiver: Self,
                message: Tick.Instance,
                sender: Self);
        });

        Receive<Tick>(_ =>
        {
            _ticks++;
            Console.WriteLine($"Scheduler tick: {_ticks}");

            if (_ticks >= 5)
            {
                _cancelable?.Cancel();
                _cancelable = null;
                Console.WriteLine("Scheduler finished after 5 ticks.");
            }
        });
    }

    protected override void PostStop()
    {
        _cancelable?.Cancel();
        base.PostStop();
    }
}
