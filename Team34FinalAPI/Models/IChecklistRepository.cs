using Team34FinalAPI.Models;

namespace Team34FinalAPI.Models
{
    public interface IChecklistRepository
    {
        Task AddChecklistAsync(VehicleChecklist checklist);
        Task AddPostChecklistAsync(PostChecklist postchecklist);
        Task<PostChecklist> GetLastVehicleChecklistAsync(int vehicleId);

        Task<bool> SaveChangesAsync();
    }
}
