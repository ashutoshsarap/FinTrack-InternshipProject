using Microsoft.AspNetCore.Identity;

namespace FinTrack.Models.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
