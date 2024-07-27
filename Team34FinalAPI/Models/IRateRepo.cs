namespace Team34FinalAPI.Models
{
    public interface IRateRepo
    {
        Task<bool> CreateRate(Rate rate); // C
        Task<Rate> GetRate(int RateID); // R - one
        Task<bool> UpdateRate(Rate rate); // U
                                          // 
        Task<Rate> AddRateAsync(Rate rate);
    }
}
