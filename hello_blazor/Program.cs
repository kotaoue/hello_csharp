using hello_blazor.Components;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("/today", () =>
{
        var today = DateTime.Now.ToString("yyyy年M月d日 (dddd)", CultureInfo.GetCultureInfo("ja-JP"));
        var html = $$"""
<!doctype html>
<html lang="ja">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>今日の日付</title>
    <style>
        body { font-family: sans-serif; max-width: 640px; margin: 2rem auto; padding: 0 1rem; }
        .card { background: #f4f6f8; border-radius: .5rem; padding: 1rem; font-size: 1.2rem; font-weight: 700; }
        a { color: #0b57d0; }
    </style>
</head>
<body>
    <h1>今日の日付</h1>
    <p class="card">{{today}}</p>
    <p><a href="/">トップに戻る</a></p>
</body>
</html>
""";

        return Results.Content(html, "text/html; charset=utf-8");
});

app.Run();
