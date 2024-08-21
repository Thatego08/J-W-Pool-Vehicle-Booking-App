using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Models
{
    public interface IFAQRepo
    {
        Task<IEnumerable<FAQVM>> GetAllFaqs();
        Task<FAQVM> GetFaqById(int id);
        Task AddFaq(FAQVM faq);
        Task UpdateFaq(FAQVM faq);
        Task DeleteFaq(int id);
        Task PostFaqToWebsite(int id);
        Task<IEnumerable<FAQVM>> GetWebsiteFaqs();
    }
}
