using FinTrack.Data;
using FinTrack.Models.Entity;
using FinTrack.Repository.IRepository;

namespace FinTrack.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUser
    {

        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
                _db = db;
        }
    }
}
