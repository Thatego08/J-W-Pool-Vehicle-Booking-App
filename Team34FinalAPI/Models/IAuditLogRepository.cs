namespace Team34FinalAPI.Models
{
    public interface IAuditLogRepository
    {
        Task AddLogAsync(AuditLog log);
        Task<IEnumerable<AuditLog>> GetLogsAsync();
    }
}
