using Microsoft.AspNetCore.Identity;

namespace WebApplication_sixteen_clothing.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
