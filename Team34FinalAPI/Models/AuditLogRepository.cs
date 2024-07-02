using Microsoft.EntityFrameworkCore;

namespace Team34FinalAPI.Models
{
    public class AuditLogRepository:IAuditLogRepository
    {
        private readonly UserDbContext _context;

        public AuditLogRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task AddLogAsync(AuditLog log)
        {
            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetLogsAsync()
        {
            return await _context.AuditLogs.ToListAsync();
        }
    }
}
