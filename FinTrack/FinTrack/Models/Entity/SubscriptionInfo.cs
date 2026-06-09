using FinTrack.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinTrack.Models.Entity
{
    public class SubscriptionInfo
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string? UserId { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; } = SubscriptionPlan.Free;
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
