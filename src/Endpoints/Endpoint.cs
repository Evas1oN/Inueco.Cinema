namespace Inueco.Cinema.Endpoints;
using Microsoft.EntityFrameworkCore;

public static class Endpoints {

    public static void Register<T> (this WebApplication app, string route) where T : class {
        var group = app.MapGroup(route);

        group.MapGet("/", async (CinemaContext db, string? include = null) => {
            var query = db.Set<T>().AsQueryable();
            if (include != null) query = query.Include(include);

            return TypedResults.Ok(await query.ToListAsync());
        });

        group.MapPost("/", async (CinemaContext db, T item) => {
            await db.AddAsync(item);
            await db.SaveChangesAsync();
            return TypedResults.Ok(item);
        });

        group.MapDelete("/{id}", async (CinemaContext db, Guid id) => {
            var item = await db.FindAsync<T>(id);
            if (item == null) return TypedResults.NotFound();

            db.Remove(item);
            await db.SaveChangesAsync();
            return Results.Ok();
        });

        group.MapPut("/", async (CinemaContext db, T item) => {
            if (!db.Set<T>().Any(x => x == item)) return Results.NotFound();
            db.Update(item);
            await db.SaveChangesAsync();
            return TypedResults.Ok(item);
        });
    }
}
