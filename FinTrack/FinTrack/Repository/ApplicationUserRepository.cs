using FinTrack.Data;
using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;

namespace FinTrack.Repository
{
    public class IApplicationUserRepository : Repository<ApplicationUser>, IApplicationUser
    {

        private readonly ApplicationDbContext _db;
        public IApplicationUserRepository(ApplicationDbContext db, ICurrentUserService currentUserService) : base(db, currentUserService)
        {
                _db = db;
        }
    }
}
