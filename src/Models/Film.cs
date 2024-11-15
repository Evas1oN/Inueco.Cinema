namespace Inueco.Cinema.Models;

public class Film {
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required Guid GenreId { get; set; }
    public Genre? Genre { get; set; }
}