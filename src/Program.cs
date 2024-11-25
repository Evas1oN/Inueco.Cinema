using Inueco.Cinema;
using Inueco.Cinema.Endpoints;
using Inueco.Cinema.Models;
using Inueco.Cinema.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var DB_PATH = Environment.GetEnvironmentVariable("DB_PATH") ?? "./db.sqlite";

builder.Services.AddDbContext<CinemaContext>(options => {
    options.UseSqlite($"Data Source = {DB_PATH}");
});
builder.Services.AddSingleton<UserSessionService>();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapFallbackToFile("index.html")
.AllowAnonymous();


app.Register<Genre>("/api/genres");
app.Register<Film>("/api/films");
app.Register<Session>("/api/sessions");
app.RegisterAuthService("/api/auth");
app.RegisterUserEndpoint("/api/users");

var favourites = app.MapGroup("/api/favourites");

favourites.MapPost("/{userId}", async (CinemaContext db, Guid userId, Guid filmId, UserSessionService session, HttpContext context) => {
    var token = context.Request.Cookies.FirstOrDefault(x => x.Key == "user.token").Value;

    if (token == null) return Results.Unauthorized();

    if (!session.ValidateToken(Guid.Parse(token), out User auth)) return Results.Unauthorized();

    if (userId != auth.Id) return Results.Forbid();

    var queryUser = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);

    if (queryUser == null) return Results.NotFound();

    var queryFilm = await db.Films.FirstOrDefaultAsync(f => f.Id == filmId);

    if (queryFilm == null) return Results.NotFound();

    if (queryUser.FavouriteFilms.Any(x => x.Id == filmId)) return Results.NoContent();

    queryUser.FavouriteFilms.Add(queryFilm);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();