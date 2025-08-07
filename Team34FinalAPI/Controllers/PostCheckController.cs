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

            if (pcvm.MediaFiles != null && pcvm.MediaFiles.Count > 0)
            {
                foreach (var file in pcvm.MediaFiles)
                {
                    if (file != null)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await file.CopyToAsync(ms);
                            var fileBytes = ms.ToArray();

                            var tripMedia = new TripMedia
                            {
                                Description = pcvm.MediaDescription,
                                FileName = file.FileName,
                                FileContent = fileBytes,
                                MediaType = file.ContentType
                            };

                            postCheck.TripMedia.Add(tripMedia);
                        }
                    }
                }
            }

            try
            {
                _context.PostChecks.Add(postCheck);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                var innerExceptionMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                return StatusCode(500, $"Database update error: {innerExceptionMessage}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }

            return Ok(postCheck);
        }


        [HttpGet("{postCheckId}")]
        public async Task<IActionResult> GetPostCheckByIdAsync(int postCheckId)
        {
            var postCheck = await _context.PostChecks
                                           .Include(pc => pc.TripMedia)
                                           .FirstOrDefaultAsync(pc => pc.PostCheckId == postCheckId);

            if (postCheck == null)
            {
                return NotFound();
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
                    tm.MediaId,
                    tm.FileName,
                    tm.MediaType,
                    FileContent = Convert.ToBase64String(tm.FileContent)
                }).ToList()
            };

            return Ok(postCheckDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllPostChecks")]
        public async Task<IActionResult> GetAllPostChecksAsync()
        {
            var postChecks = await _context.PostChecks
                                            .Include(pc => pc.TripMedia)
                                            .ToListAsync();

            if (postChecks == null || !postChecks.Any())
            {
                return NotFound();
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
                    tm.MediaId,
                    tm.FileName,
                    tm.MediaType,
                    FileContent = Convert.ToBase64String(tm.FileContent)
                }).ToList()
            }).ToList();

            return Ok(postCheckDtos);
        }

        [HttpDelete("{postCheckId}")]
        public async Task<IActionResult> DeletePostCheckAsync(int postCheckId)
        {
            var postCheck = await _context.PostChecks
                                          .Include(pc => pc.TripMedia)
                                          .FirstOrDefaultAsync(pc => pc.PostCheckId == postCheckId);

            if (postCheck == null)
            {
                return NotFound();
            }

            try
            {
                if (postCheck.TripMedia != null)
                {
                    _context.TripMedia.RemoveRange(postCheck.TripMedia);
                }

                _context.PostChecks.Remove(postCheck);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException dbEx)
            {
                var innerExceptionMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                return StatusCode(500, $"Database update error: {innerExceptionMessage}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }
    }
}
