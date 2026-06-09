using FinTrack.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinTrack.Models.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; } = SubscriptionPlan.Free;
    }
}
