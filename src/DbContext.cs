using Inueco.Cinema.Models;
using Microsoft.EntityFrameworkCore;

namespace Inueco.Cinema;

public class CinemaContext : DbContext
{
    public DbSet<Film> Films => Set<Film>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Session> Sessions => Set<Session>();
    public CinemaContext(DbContextOptions options) : base(options) { 
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Film>().Navigation(e => e.Genre).AutoInclude();
        modelBuilder.Entity<Session>().Navigation(f => f.Film).AutoInclude();
    }
}