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

        public async Task<List<AuditLog>> GetAuditLogsAsync(string username = null)
        {
            if (string.IsNullOrEmpty(username))
            {
                return await _context.AuditLogs.ToListAsync(); // Retrieve all logs
            }
            return await _context.AuditLogs
                                 .Where(log => log.UserName.Contains(username))
                                 .ToListAsync(); // Filter logs by username
        }
        public async Task EditAuditLogDetailsAsync(int auditLogId, string newDetails)
        {
            var auditLog = await _context.AuditLogs.FindAsync(auditLogId);
            if (auditLog != null)
            {
                auditLog.Details = newDetails;
                auditLog.Timestamp = DateTime.UtcNow; // Update timestamp to current time
                _context.AuditLogs.Update(auditLog);
                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteAuditLogAsync(int auditLogId)
        {
            var auditLog = await _context.AuditLogs.FindAsync(auditLogId);
            if (auditLog != null)
            {
                _context.AuditLogs.Remove(auditLog);
                await _context.SaveChangesAsync();
            }
        }

    }
}
