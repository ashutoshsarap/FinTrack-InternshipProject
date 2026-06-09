using FinTrack.Models.Entity;

namespace FinTrack.Service.IService
{
    public interface IUpdateUserSubscriptionPlan
    {
        public Task UpgradeSubscriptionPlan(ApplicationUser applicationUser);
        public Task DowngradeSubscriptionPlan(ApplicationUser applicationUser);
    }
}
