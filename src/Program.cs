using Inueco.Cinema;
using Inueco.Cinema.Endpoints;
using Inueco.Cinema.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var DB_PATH = Environment.GetEnvironmentVariable("DB_PATH") ?? "./db.sqlite";

builder.Services.AddDbContext<CinemaContext>(options => {
    options.UseSqlite($"Data Source = {DB_PATH}");
});
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapFallbackToFile("index.html").AllowAnonymous();


app.Register<Genre>("/api/genres");
app.Register<Film>("/api/films");
app.Register<Session>("/api/sessions");

app.Run();