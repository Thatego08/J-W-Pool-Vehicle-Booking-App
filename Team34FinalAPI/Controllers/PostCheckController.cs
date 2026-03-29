using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;
using iText.Kernel.Counter.Context;
using Microsoft.AspNetCore.Authorization;

namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostCheckController : ControllerBase
    {
        private readonly TripDbContext _context;
        private readonly ILogger<PostCheckController> _logger;

        public PostCheckController(TripDbContext context, ILogger<PostCheckController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpPost("CreatePostCheck")]
        [RequestSizeLimit(10_000_000)]
        public async Task<IActionResult> CreatePostCheck([FromForm] PostCheckViewModel pcvm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pcvm == null)
            {
                return BadRequest("PostCheckViewModel cannot be null");
            }

            // 1. Fetch the trip first. 
            // This ensures TripId is valid AND loads the trip into the tracker for the TravelEnd update.
            var trip = await _context.Trips.FindAsync(pcvm.TripId);

            if (trip == null)
            {
                // If this hits, it means pcvm.TripId is likely 0 or 
                // doesn't match a real ID in the 'Trips' table.
                return BadRequest($"Trip ID {pcvm.TripId} not found in the 'Trips' table. Please verify the Trip ID being sent from the frontend.");
            }

            var postCheck = new PostCheck
            {
                TripId = pcvm.TripId,
                ClosingKms = pcvm.ClosingKms,
                OilLeaks = pcvm.OilLeaks,
                FuelLevel = pcvm.FuelLevel,
                Mirrors = pcvm.Mirrors,
                SunVisor = pcvm.SunVisor,
                SeatBelts = pcvm.SeatBelts,
                HeadLights = pcvm.HeadLights,
                Indicators = pcvm.Indicators,
                ParkLights = pcvm.ParkLights,
                BrakeLights = pcvm.BrakeLights,
                StrobeLight = pcvm.StrobeLight,
                ReverseLight = pcvm.ReverseLight,
                ReverseHooter = pcvm.ReverseHooter,
                Horn = pcvm.Horn,
                WindscreenWiper = pcvm.WindscreenWiper,
                TyreCondition = pcvm.TyreCondition,
                SpareWheelPresent = pcvm.SpareWheelPresent,
                JackAndWheelSpannerPresent = pcvm.JackAndWheelSpannerPresent,
                Brakes = pcvm.Brakes,
                Handbrake = pcvm.Handbrake,
                JWMarketingMagnets = pcvm.JWMarketingMagnets,
                CheckedByJWSecurity = pcvm.CheckedByJWSecurity,
                LicenseDiskValid = pcvm.LicenseDiskValid,
                Comments = pcvm.Comments,
                AdditionalComments = pcvm.AdditionalComments,
                TripMedia = new List<TripMedia>()
            };

            // Handle MediaFiles
            if (pcvm.MediaFiles != null && pcvm.MediaFiles.Count > 0)
            {
                foreach (var file in pcvm.MediaFiles)
                {
                    if (file != null)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await file.CopyToAsync(ms);
                            postCheck.TripMedia.Add(new TripMedia
                            {
                                Description = pcvm.MediaDescription,
                                FileName = file.FileName,
                                FileContent = ms.ToArray(),
                                MediaType = file.ContentType
                            });
                        }
                    }
                }
            }

            try
            {
                // 2. Add the PostCheck
                _context.PostChecks.Add(postCheck);

                // 3. Update the Trip's end time. 
                // If the user didn't provide one from the frontend, we use the current time.
                trip.TravelEnd = pcvm.TravelEnd ?? DateTime.Now;

                await _context.SaveChangesAsync();
                return Ok(postCheck);
            }
            catch (DbUpdateException dbEx)
            {
                var innerExceptionMessage = dbEx.InnerException?.Message ?? dbEx.Message;

                // This is where that 23503 error was being caught.
                // If the SQL Constraint fix was applied, this shouldn't trigger anymore.
                return StatusCode(500, $"Database update error: {innerExceptionMessage}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }

        [HttpGet("{postCheckId}")]
        public async Task<IActionResult> GetPostCheckByIdAsync(int postCheckId)
        {
            var postCheck = await _context.PostChecks
                                           .Include(pc => pc.TripMedia) // Include related TripMedia
                                           .FirstOrDefaultAsync(pc => pc.PostCheckId == postCheckId); // Use PostCheckId

            if (postCheck == null)
            {
                return NotFound(); // Return 404 if PostCheck not found
            }

            var postCheckDto = new
            {
                postCheck.PostCheckId,
                postCheck.TripId,
                postCheck.ClosingKms,
                postCheck.OilLeaks,
                postCheck.FuelLevel,
                postCheck.Mirrors,
                postCheck.SunVisor,
                postCheck.SeatBelts,
                postCheck.HeadLights,
                postCheck.Indicators,
                postCheck.ParkLights,
                postCheck.BrakeLights,
                postCheck.StrobeLight,
                postCheck.ReverseLight,
                postCheck.ReverseHooter,
                postCheck.Horn,
                postCheck.WindscreenWiper,
                postCheck.TyreCondition,
                postCheck.SpareWheelPresent,
                postCheck.JackAndWheelSpannerPresent,
                postCheck.Brakes,
                postCheck.Handbrake,
                postCheck.JWMarketingMagnets,
                postCheck.CheckedByJWSecurity,
                postCheck.LicenseDiskValid,
                postCheck.Comments,
                postCheck.AdditionalComments,
                TripMedia = postCheck.TripMedia.Select(tm => new
                {
                    tm.MediaId, // Adjust if necessary
                    tm.FileName,
                    tm.MediaType,
                    // Serve URL or Base64 for images
                    FileContent = Convert.ToBase64String(tm.FileContent)
                }).ToList()
            };

            return Ok(postCheckDto); // Return PostCheck along with media
        }
       // [Authorize(Roles = "Admin")]
        [HttpGet("GetAllPostChecks")]
        public async Task<IActionResult> GetAllPostChecksAsync()
        {
            var postChecks = await _context.PostChecks
                                            .Include(pc => pc.TripMedia) // Include related TripMedia
                                            .ToListAsync(); // Fetch all PostChecks

            if (postChecks == null || !postChecks.Any())
            {
                return NotFound(); // Return 404 if no PostChecks are found
            }

            var postCheckDtos = postChecks.Select(postCheck => new
            {
                postCheck.PostCheckId,
                postCheck.TripId,
                postCheck.ClosingKms,
                postCheck.OilLeaks,
                postCheck.FuelLevel,
                postCheck.Mirrors,
                postCheck.SunVisor,
                postCheck.SeatBelts,
                postCheck.HeadLights,
                postCheck.Indicators,
                postCheck.ParkLights,
                postCheck.BrakeLights,
                postCheck.StrobeLight,
                postCheck.ReverseLight,
                postCheck.ReverseHooter,
                postCheck.Horn,
                postCheck.WindscreenWiper,
                postCheck.TyreCondition,
                postCheck.SpareWheelPresent,
                postCheck.JackAndWheelSpannerPresent,
                postCheck.Brakes,
                postCheck.Handbrake,
                postCheck.JWMarketingMagnets,
                postCheck.CheckedByJWSecurity,
                postCheck.LicenseDiskValid,
                postCheck.Comments,
                postCheck.AdditionalComments,
                TripMedia = postCheck.TripMedia.Select(tm => new
                {
                    tm.MediaId, // Adjust if necessary
                    tm.FileName,
                    tm.MediaType,
                    // Serve URL or Base64 for images
                    FileContent = Convert.ToBase64String(tm.FileContent)
                }).ToList()
            }).ToList();

            return Ok(postCheckDtos); // Return the list of PostChecks with media
        }


        [HttpDelete("{postCheckId}")]
        public async Task<IActionResult> DeletePostCheckAsync(int postCheckId)
        {
            var postCheck = await _context.PostChecks
                                          .Include(pc => pc.TripMedia)
                                          .FirstOrDefaultAsync(pc => pc.PostCheckId == postCheckId);

            if (postCheck == null)
            {
                return NotFound(); // Return 404 if PostCheck not found
            }

            try
            {
                // Remove associated media files
                if (postCheck.TripMedia != null)
                {
                    _context.TripMedia.RemoveRange(postCheck.TripMedia);
                }

                // Remove the post check
                _context.PostChecks.Remove(postCheck);
                await _context.SaveChangesAsync();

                return NoContent(); // Return 204 No Content on successful deletion
            }
            catch (DbUpdateException dbEx)
            {
                var innerExceptionMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                return StatusCode(500, $"Database update error: {innerExceptionMessage}"); // Returns in case of database update exception
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}"); // Returns in case of other exceptions
            }
        }

    }
}