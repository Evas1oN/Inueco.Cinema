namespace Inueco.Cinema.Endpoints;

using System.Text;
using Inueco.Cinema.Models;
using Inueco.Cinema.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

public static class UserEndpoint {

    private const string SALT = "c2FsdAo=";
    private const string COOKIE_NAME = "user.token";

    

    private static string HashPassword(string password) {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(SALT),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 512 / 8
        ));
    }

    public static void RegisterAuthService (this WebApplication app, string route) {
        var group = app.MapGroup(route);

        group.MapGet("/", (CinemaContext db, UserSessionService session, HttpContext context) => {
            string token = context.Request.Cookies.FirstOrDefault(x => x.Key == COOKIE_NAME).Value;

            if (token == null) {
                return Results.Unauthorized();
            }

            if (session.ValidateToken(Guid.Parse(token), out User user)) {
                user.Password = "[REDACTED]";
                return TypedResults.Ok(user);
            }
            else {
                context.Response.Cookies.Delete(COOKIE_NAME);
            }

            return Results.Unauthorized();
        });

        group.MapPost("/", async (CinemaContext db, User user, UserSessionService session, HttpContext httpContext) => {
            var query = await db.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

            if (query == null) {
                return Results.NotFound();
            }

            if (query.Password != HashPassword(user.Password)) {
                return Results.Unauthorized();
            }

            var token = session.CreateToken(query);
            
            CookieOptions cookieOptions = new() {
                SameSite= SameSiteMode.Strict,
                Secure = true
            };

            httpContext.Response.Cookies.Append(COOKIE_NAME, token.ToString(), cookieOptions);
            return TypedResults.Ok(query);
        });
    }

    public static void RegisterUserEndpoint (this WebApplication app, string route) {
        var group = app.MapGroup(route);

        group.MapPost("/", async (CinemaContext db, User user, UserSessionService session) => {
            user.Password = HashPassword(user.Password);
            await db.Users.AddAsync(user);
            session.CreateToken(user);
            await db.SaveChangesAsync();
            return TypedResults.Ok(user);
        });

        group.MapGet("/", async (CinemaContext db) => {
            var query = db.Users.AsQueryable();
            return TypedResults.Ok(await query.ToListAsync());
        });
    }
}
