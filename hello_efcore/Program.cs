using Microsoft.EntityFrameworkCore;

Console.WriteLine(new string('-', 64));
Console.WriteLine("Hello, EF Core!");

await using var db = new HelloDbContext();
await db.Database.EnsureCreatedAsync();

if (!await db.Messages.AnyAsync())
{
    db.Messages.AddRange(
        new GreetingMessage { Text = "Welcome to EF Core", CreatedAtUtc = DateTime.UtcNow },
        new GreetingMessage { Text = "SQLite local database is ready", CreatedAtUtc = DateTime.UtcNow },
        new GreetingMessage { Text = $"{DateTime.Today.Year + DateTime.Today.Month + DateTime.Today.Day}", CreatedAtUtc = DateTime.UtcNow },
        new GreetingMessage { Text = $"Daily code {DateTime.Today:yyyyMMdd}: {(DateTime.Today.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday ? "Weekend" : "Weekday")} #{(DateTime.Today.Year + DateTime.Today.Month * DateTime.Today.Day) % 97:D2}", CreatedAtUtc = DateTime.UtcNow }
    );
    await db.SaveChangesAsync();
}

var randomValue = Random.Shared.Next(100000, 1000000);
var payload = $"{DateTime.UtcNow:O}-{Random.Shared.NextInt64()}";
var shortSha256 = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(payload)))[..16];

db.Messages.AddRange(
    new GreetingMessage { Text = $"Random value: {randomValue}", CreatedAtUtc = DateTime.UtcNow },
    new GreetingMessage { Text = $"SHA-256: {shortSha256}", CreatedAtUtc = DateTime.UtcNow }
);
await db.SaveChangesAsync();

var messages = await db.Messages
    .OrderBy(x => x.Id)
    .ToListAsync();

Console.WriteLine($"Saved messages: {messages.Count}");
foreach (var message in messages)
{
    Console.WriteLine($"[{message.Id}] {message.Text} ({message.CreatedAtUtc:yyyy-MM-dd HH:mm:ss} UTC)");
}

Console.WriteLine(new string('-', 64));
Console.WriteLine("Done.");

sealed class HelloDbContext : DbContext
{
    public DbSet<GreetingMessage> Messages => Set<GreetingMessage>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=hello_efcore.db");
    }
}

sealed class GreetingMessage
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
