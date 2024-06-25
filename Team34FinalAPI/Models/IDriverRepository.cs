namespace Team34FinalAPI.Models
{
    public interface IDriverRepository
    {
        Task<bool> SaveChangesAync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<Driver[]> GetAllDriverAsync();
        Task<Driver> GetDriverAsync(string userName);
    }
}
