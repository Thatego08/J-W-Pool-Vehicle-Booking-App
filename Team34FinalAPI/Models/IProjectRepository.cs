namespace Team34FinalAPI.Models
{
    public interface IProjectRepository
    {
        void Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        //Project

        Task AddProjectAsync(Project project);
        Task<Project[]> GetAllProjectsAsync();
        Task<Project> GetProjectAsync(int ProjectID);

        Task<IEnumerable<Project>> GetProjectsAsync();
        Task UpdateProjectAsync(Project project);


        Task<Project> GetProjectByNumberAsync(int projectNumber);
    }
}
