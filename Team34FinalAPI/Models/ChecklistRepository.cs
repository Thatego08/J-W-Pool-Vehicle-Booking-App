using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Models
{
    public class ChecklistRepository : IChecklistRepository
    {
        private readonly VehicleDbContext _context;

        public ChecklistRepository(VehicleDbContext context)
        {
            _context = context;
        }

        public async Task AddChecklistAsync(VehicleChecklist vehicleChecklist)
        {
            await _context.VehicleChecklists.AddAsync(vehicleChecklist);
            await _context.SaveChangesAsync();
        }

        public async Task AddPostChecklistAsync(PostChecklist postChecklist)
        {
            try
            {
                // Get the last checklist for the vehicle
                var lastChecklist = await _context.PostChecklist
                    .Where(vc => vc.VehicleId == postChecklist.VehicleId)
                    .OrderByDescending(vc => vc.PostId)
                    .FirstOrDefaultAsync();

                // Set the OpeningKms to the ClosingKms of the last checklist
                if (lastChecklist != null)
                {
                    postChecklist.OpeningKms = lastChecklist.ClosingKms;
                }

                // Add the new PostChecklist
                await _context.PostChecklist.AddAsync(postChecklist);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Rethrow or handle as needed
                throw;
            }
        }


        public async Task<PostChecklist> GetLastVehicleChecklistAsync(int vehicleId)
        {
            return await _context.PostChecklist
                .Where(pc => pc.VehicleId == vehicleId)
                .OrderByDescending(pc => pc.PostId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
