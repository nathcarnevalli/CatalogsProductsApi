
using Microsoft.AspNetCore.Identity;

namespace APICatalogo.Models;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken {  get; set; }

    public DateTime RefreshTokenExpiredTime { get; set; }
}
