using FinTrack.Data;
using FinTrack.Models.Entity;

namespace FinTrack.Service.IService
{
    public interface IAuditService
    {        
        public Task LogAuditDataAsync(AuditData auditData);

    }
}
