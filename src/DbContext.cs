using Inueco.Cinema.Models;
using Microsoft.EntityFrameworkCore;

namespace Inueco.Cinema;

public class CinemaContext : DbContext
{
    public DbSet<Film> Films => Set<Film>();
    public DbSet<Genre> Genres => Set<Genre>();
    public CinemaContext(DbContextOptions options) : base(options) { 
        Database.EnsureCreated();
    }
}