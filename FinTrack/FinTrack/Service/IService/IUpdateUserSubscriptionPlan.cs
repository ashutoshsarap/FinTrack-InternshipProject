namespace FinTrack.Service.IService
{
    public interface IUpdateUserSubscriptionPlan
    {
        public void UpgradeSubscriptionPlan(string userId);
        public void DowngradeSubscriptionPlan(string userId);
    }
}
