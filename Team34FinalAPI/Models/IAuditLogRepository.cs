namespace Team34FinalAPI.Models
{
    public interface IAuditLogRepository
    {
        Task AddLogAsync(AuditLog log);
        Task<IEnumerable<AuditLog>> GetLogsAsync();


        Task<List<AuditLog>> GetAuditLogsAsync(string username = null);


        Task EditAuditLogDetailsAsync(int auditLogId, string newDetails);
        Task DeleteAuditLogAsync(int auditLogId);
    }
}
