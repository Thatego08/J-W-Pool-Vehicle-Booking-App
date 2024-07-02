namespace Team34FinalAPI.Models
{
    public interface IFeedbackRepository
    {
        Task AddFeedbackAsync(Feedback feedback);
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync();
    }
}
