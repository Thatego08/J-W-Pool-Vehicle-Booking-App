namespace Team34FinalAPI.Models
{
    public interface IRateRepo
    {
        Task<bool> CreateRate(Rate rate);
        Task<bool> UpdateRate(Rate rate);
    }
}
