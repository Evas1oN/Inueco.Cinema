namespace Inueco.Cinema.Models;

public class Session {
    public Guid Id { get; set; }
    public Guid FilmId { get; set; }
    public Film? Film { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
}