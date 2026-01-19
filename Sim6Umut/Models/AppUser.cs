using Microsoft.AspNetCore.Identity;

namespace Sim6Umut.Models
{
    public class AppUser:IdentityUser
    {
        public string Fullname { get; set; } = string.Empty;
    }
}
