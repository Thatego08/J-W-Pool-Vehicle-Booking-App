using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Controllers
{
    //Comment to disable locking
    [Authorize(Roles = "Admin,Driver")]
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ILogger<FeedbackController> _logger;

        public FeedbackController(IFeedbackRepository feedbackRepository, ILogger<FeedbackController> logger)
        {
            _feedbackRepository = feedbackRepository ?? throw new ArgumentNullException(nameof(feedbackRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackViewModel model)
        {
            if (model == null)
            {
                return BadRequest("Feedback model is null.");
            }

            if (model.Rating < 1 || model.Rating > 5)
            {
                return BadRequest("Rating must be between 1 and 5.");
            }

            var feedback = new Feedback
            {
                UserName = model.UserName,
                Email = model.Email,
                Message = model.Message,
                Timestamp = DateTime.UtcNow,
                Rating = model.Rating
            };

            try
            {
                await _feedbackRepository.AddFeedbackAsync(feedback);
                _logger.LogInformation("Feedback submitted by {UserName} with message: {Message} and rating: {Rating}", model.UserName, model.Message, model.Rating);
                return Ok(new { Message = "Feedback submitted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while submitting feedback.");
                return StatusCode(500, "Internal server error. Please contact support.");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            try
            {
                var feedbacks = await _feedbackRepository.GetAllFeedbacksAsync();
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving feedbacks.");
                return StatusCode(500, "Internal server error. Please contact support.");
            }
        }

    }
}
