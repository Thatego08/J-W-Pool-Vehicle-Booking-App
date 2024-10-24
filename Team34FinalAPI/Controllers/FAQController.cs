using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Team34FinalAPI.Migrations.BookingDb;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQController : ControllerBase
    {
        private readonly IFAQRepo _faqRepo;
        private readonly IAuditLogRepository _auditLogRepo;

        public FAQController(IFAQRepo faqRepo, IAuditLogRepository auditLogRepo)
        {
            _faqRepo = faqRepo;
            _auditLogRepo = auditLogRepo;
        }

        // get all faqs
        [HttpGet("GetAllFAQs")]
        public async Task<IActionResult> GetAllFaqs()
        {
            var faqs = await _faqRepo.GetAllFaqs();
            return Ok(faqs);
        }

        // get faq by id
        [HttpGet("GetFAQ/{id}")]
        public async Task<IActionResult> GetFaqById(int id)
        {
            var faq = await _faqRepo.GetFaqById(id);
            if (faq == null)
            {
                return NotFound(new { Message = "FAQ Not Found" });
            }
            return Ok(faq);
        }

        // add new faq
        [HttpPost("AddFAQ")]
        public async Task<IActionResult> AddFaq([FromBody] FAQVM faq)
        {
            if (faq == null)
            {
                return BadRequest("FAQ object is null");
            }

            //Audit Log stuff 
            await _auditLogRepo.AddLogAsync(new AuditLog
            {
               UserName = "Admin",
                Action = "New FAQ Added",
                Details = $"New Frequently Asked Question Added by an Administrator" ,
                Timestamp = DateTime.UtcNow
            });

            await _faqRepo.AddFaq(faq);
            return CreatedAtAction(nameof(GetFaqById), new { id = faq.FAQId, Message = "FAQ added successfully!" }, faq);
        }

        // update existing faq
        [HttpPut("UpdateFAQ/{id}")]
        public async Task<IActionResult> UpdateFaq(int id, [FromBody] FAQVM faq)
        {
            if (faq == null || faq.FAQId != id)
            {
                return BadRequest(new { Message = "Invalid FAQ" });
            }

            var existingFaq = await _faqRepo.GetFaqById(id);
            if (existingFaq == null)
            {
                return NotFound();
            }

            await _faqRepo.UpdateFaq(faq);


            //Audit Log stuff 
            await _auditLogRepo.AddLogAsync(new AuditLog
            {
                UserName = "Admin",
                Action = "FAQ Details Updated",
                Details = $"Frequently Asked Question Details updated by an Administrator",
                Timestamp = DateTime.UtcNow
            });
            return Ok(new { Message = "FAQ Updated Successfully!" });

        }

        //delete existing faq
        [HttpDelete("DeleteFAQ/{id}")]
        public async Task<IActionResult> DeleteFaq(int id)
        {
            var faqToDelete = await _faqRepo.GetFaqById(id);
            if (faqToDelete == null)
            {
                return NotFound(new { Message = "Invalid FAQ object" });
            }

            await _faqRepo.DeleteFaq(id);


            //Audit Log stuff 
            await _auditLogRepo.AddLogAsync(new AuditLog
            {
                UserName = "Admin",
                Action = "FAQ Deleted",
                Details = $"Frequently Asked Question Deleted by an Administrator",
                Timestamp = DateTime.UtcNow
            });
            return Ok(new { Message = "FAQ Deleted Successfully!" });
        }

        //post faq to the website
        [HttpPost("PostFaqToWebsite/{id}")]
        public async Task<IActionResult> PostFaqToWebsite(int id)
        {
            var faq = await _faqRepo.GetFaqById(id);
            if (faq == null)
            {
                return NotFound(new { Message = "FAQ Not Found" });
            }

            await _faqRepo.PostFaqToWebsite(id);
            return Ok(new { Message = "FAQ posted to home page successfully!" });
        }

        // get FAQs posted to website
        [HttpGet("GetWebsiteFAQs")]
        public async Task<IActionResult> GetWebsiteFaqs()
        {
            var faqs = await _faqRepo.GetWebsiteFaqs();
            return Ok(faqs);
        }


    
}
}
