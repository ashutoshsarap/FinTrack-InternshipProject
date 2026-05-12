using FinTrack.Data;
using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;

namespace FinTrack.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUser
    {

        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db, ICurrentUserService currentUserService) : base(db, currentUserService)
        {
                _db = db;
        }
    }
}
