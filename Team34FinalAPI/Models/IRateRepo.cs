using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Models
{
    public interface IRateRepo
    {
        Task<IEnumerable<RateViewModel>> GetAllRatesAsync();
        Task<RateViewModel> GetRateByIdAsync(int rateId);
        Task<Rate> CreateRateAsync(Rate rate);
        Task<Rate> UpdateRateAsync(Rate rate);
        //Task<IEnumerable<RateType>> GetAllRateTypesAsync();
    }
}
