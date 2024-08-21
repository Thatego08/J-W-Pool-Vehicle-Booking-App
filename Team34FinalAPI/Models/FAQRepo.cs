using Microsoft.EntityFrameworkCore;
using Team34FinalAPI.Data;
using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Models
{
    public class FAQRepo : IFAQRepo
    {
        private readonly AppDbContext _appDbContext;

        public FAQRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<FAQVM>> GetAllFaqs()
        {
            var faqs = await _appDbContext.Faqs
                .Select(faq => new FAQVM
                {
                    FAQId = faq.FAQId,
                    Question = faq.Question,
                    Answer = faq.Answer,
                   
                })
                .ToListAsync();

            return faqs;
        }

        public async Task<FAQVM> GetFaqById(int id)
        {
            var faq = await _appDbContext.Faqs
                .Where(f => f.FAQId == id)
                .Select(f => new FAQVM
                {
                    FAQId = f.FAQId,
                    Question = f.Question,
                    Answer = f.Answer,
                  
                })
                .FirstOrDefaultAsync();

            return faq;
        }

        public async System.Threading.Tasks.Task AddFaq(FAQVM faq)
        {
            var newFaq = new FAQ
            {
                Question = faq.Question,
                Answer = faq.Answer,
             
            };

            _appDbContext.Faqs.Add(newFaq);
            await _appDbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateFaq(FAQVM faq)
        {
            var faqToUpdate = await _appDbContext.Faqs.FindAsync(faq.FAQId);
            if (faqToUpdate == null)
            {
                return;
            }

            faqToUpdate.Question = faq.Question;
            faqToUpdate.Answer = faq.Answer;
            

            _appDbContext.Entry(faqToUpdate).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteFaq(int id)
        {
            var faqToDelete = await _appDbContext.Faqs.FindAsync(id);
            if (faqToDelete == null)
            {
                return;
            }

            _appDbContext.Faqs.Remove(faqToDelete);
            await _appDbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task PostFaqToWebsite(int id)
        {
            var faq = await _appDbContext.Faqs.FindAsync(id);
            if (faq != null)
            {
                faq.IsPostedToWebsite = true;
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<FAQVM>> GetWebsiteFaqs()
        {
            var faqs = await _appDbContext.Faqs
                .Where(f => f.IsPostedToWebsite)
                .Select(faq => new FAQVM
                {
                    FAQId = faq.FAQId,
                    Question = faq.Question,
                    Answer = faq.Answer,
                  
                })
                .ToListAsync();

            return faqs;
        }

    }
}
