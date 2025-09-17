
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Entities;

public class Member
{
    // here the member id and user id will same for same users
    public string Id { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public string? ImageUrl { get; set; }
    public required string DisplayName { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;

    public required string Gender { get; set; }
    public string? Description { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }

    // navigation property
    [JsonIgnore]
    public List<Photo> Photos { get; set; } = [];

    [JsonIgnore]
    // specify the foreign key
    [ForeignKey(nameof(Id))]
    public AppUser User { get; set; } = null!;
    // one user is related to one member
}
