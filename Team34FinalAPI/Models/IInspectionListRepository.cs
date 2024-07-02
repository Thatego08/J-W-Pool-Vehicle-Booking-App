namespace Team34FinalAPI.Models
{
    public interface IInspectionListRepository
    {
        Task AddChecklistItemAsync(InspectionList checklistItem);
        Task<InspectionList[]> GetChecklistAsync();
        Task<InspectionList> GetChecklistItemAsync(int checklistID);
        Task UpdateChecklistItemAsync(InspectionList checklistItem);
        Task<bool> SaveChangesAsync();
    }
}
