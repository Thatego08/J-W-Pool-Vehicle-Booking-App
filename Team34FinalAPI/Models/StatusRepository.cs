using Microsoft.EntityFrameworkCore;
using Team34FinalAPI.Data;

namespace Team34FinalAPI.Models
{
    public class StatusRepository : IStatusRepository
    {
        private readonly AppDbContext _context;

        public StatusRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Status>> GetAllStatusesAsync()
        {
            return await _context.Status.ToListAsync();
        }

        public async Task<Status> GetStatusByIdAsync(int statusId)
        {
            return await _context.Status.FindAsync(statusId);
        }

        public async Task AddStatusAsync(Status status)
        {
            await _context.Status.AddAsync(status);
        }

        public async Task UpdateStatusAsync(Status status)
        {
            _context.Status.Update(status);
        }

        public async Task DeleteStatusAsync(Status status)
        {
            _context.Status.Remove(status);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
