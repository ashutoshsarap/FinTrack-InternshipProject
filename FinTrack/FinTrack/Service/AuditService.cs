using FinTrack.Data;
using FinTrack.Models.Entity;
using FinTrack.Service.IService;

namespace FinTrack.Service
{
    public class AuditService : IAuditService
    {

        private readonly ApplicationDbContext _context;

        public AuditService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogAuditDataAsync(AuditData auditData)
        {
            await _context.AuditLogs.AddAsync(auditData);
            await _context.SaveChangesAsync();
        }
    }
}
