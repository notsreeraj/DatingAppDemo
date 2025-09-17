using System;

namespace API.Entities;

public class AppUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public string? ImageUrl { get; set; }
    public required byte[] PasswordHash { get; set; }

    // here passSalt is a random string to scramble the hash password so the no hashed pass is same for two users
    public required byte[] PasswordSalt { get; set; }

    // nav propery
    public Member Member { get; set; } = null!;


}
