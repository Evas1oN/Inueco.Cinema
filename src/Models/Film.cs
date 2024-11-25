using System.Text.Json.Serialization;

namespace Inueco.Cinema.Models;

public class Film {
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required Guid GenreId { get; set; }
    public string? PosterUrl { get; set; }
    public Genre? Genre { get; set; }

    [JsonIgnore]
    public List<User>? FavouriteByUsers { get; set; }
}