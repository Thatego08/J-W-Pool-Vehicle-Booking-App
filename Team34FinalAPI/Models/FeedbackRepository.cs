using Microsoft.EntityFrameworkCore;

namespace Team34FinalAPI.Models
{
    public class FeedbackRepository:IFeedbackRepository
    {
        private readonly UserDbContext _context;

        public FeedbackRepository(UserDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddFeedbackAsync(Feedback feedback)
        {
            if (feedback == null)
            {
                throw new ArgumentNullException(nameof(feedback));
            }

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            return await _context.Feedbacks.ToListAsync();
        }
    }
}
