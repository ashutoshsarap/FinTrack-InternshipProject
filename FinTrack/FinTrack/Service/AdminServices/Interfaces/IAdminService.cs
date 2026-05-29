using FinTrack.Models.AdminModelAndDtos.AdminDtos;

namespace FinTrack.Service.AdminServices.Interfaces
{
    public interface IAdminService
    {
        public AdminDashboardDto GetAdminDashboardData();
    }
}
