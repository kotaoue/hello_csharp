var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

static bool IsValidColor(string color)
{
		// Allow common CSS color inputs: named colors and hex values.
		return System.Text.RegularExpressions.Regex.IsMatch(color, "^[a-zA-Z]+$")
				|| System.Text.RegularExpressions.Regex.IsMatch(color, "^#([0-9a-fA-F]{3}|[0-9a-fA-F]{6})$");
}

app.MapGet("/", () => "Hello, World!");
app.MapGet("/greet/{name}", (string name) => $"Hello, {name}!");
app.MapGet("/add/{a:int}/{b:int}", (int a, int b) => $"{a} + {b} = {a + b}");
app.MapGet("/bg/{color}", (string color) =>
{
		if (!IsValidColor(color))
		{
				return Results.BadRequest("Invalid color. Use a named color (e.g. skyblue) or a hex color (e.g. #00ff88).");
		}

		var html = $"""
		<!doctype html>
		<html lang="en">
			<head>
				<meta charset="utf-8" />
				<meta name="viewport" content="width=device-width, initial-scale=1" />
				<title>Background Preview</title>
			</head>
			<body style="margin:0;min-height:100vh;display:grid;place-items:center;background:{color};font-family:sans-serif;">
				<h1 style="background:rgba(255,255,255,.75);padding:.6rem 1rem;border-radius:.5rem;">Background: {color}</h1>
			</body>
		</html>
		""";

		return Results.Content(html, "text/html");
});

app.Run();
