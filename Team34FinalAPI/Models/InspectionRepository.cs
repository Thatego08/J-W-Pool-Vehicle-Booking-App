using Microsoft.EntityFrameworkCore;

namespace Team34FinalAPI.Models
{
    public class InspectionRepository: IInspectionListRepository
    {
        private readonly BookingDbContext _dbContext;

        public InspectionRepository(BookingDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task AddChecklistItemAsync(InspectionList checklistItem)
        {
            if (checklistItem == null)
            {
                throw new ArgumentNullException(nameof(checklistItem));
            }

            _dbContext.InspectionLists.Add(checklistItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<InspectionList[]> GetChecklistAsync()
        {
            return await _dbContext.InspectionLists.ToArrayAsync();
        }

        public async Task<InspectionList> GetChecklistItemAsync(int checklistID)
        {
            return await _dbContext.InspectionLists.FirstOrDefaultAsync(c => c.ChecklistID == checklistID);
        }

        public async Task UpdateChecklistItemAsync(InspectionList checklistItem)
        {
            if (checklistItem == null)
            {
                throw new ArgumentNullException(nameof(checklistItem));
            }

            _dbContext.InspectionLists.Update(checklistItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}

