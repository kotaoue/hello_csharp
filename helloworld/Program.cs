using System;

class Program
{
    static void Main()
    {
        Console.WriteLine(new string('-', 64));
        Console.WriteLine("Hello, World!");
        Console.WriteLine("Welcome to C# programming.");

        Console.WriteLine(new string('-', 64));
        Console.WriteLine("Let's play Rock-Paper-Scissors!");
        string[] choices = { "✊️", "✋", "✌️" };
        Random random = new Random();
        int computerChoice = random.Next(choices.Length);
        Console.WriteLine($"Computer chose: {choices[computerChoice]}");
    }
}
