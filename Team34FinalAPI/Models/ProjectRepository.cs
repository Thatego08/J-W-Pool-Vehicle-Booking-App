using Microsoft.EntityFrameworkCore;
using System;
using Team34FinalAPI.Controllers;

namespace Team34FinalAPI.Models
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly BookingDbContext _context;
        private readonly ILogger<ProjectController> _logger;
        public ProjectRepository(BookingDbContext context, ILogger<ProjectController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;   
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task AddProjectAsync(Project project)
        {
            if (project == null)
            {
                _logger.LogError("Project is null.");
                throw new ArgumentNullException(nameof(project));
            }

            _context.Projects.Add(project);
            _logger.LogInformation("Project entity added to context.");

            await _context.SaveChangesAsync();
            _logger.LogInformation("Changes saved to database.");
        }

        public async Task<Project> GetProjectByNumberAsync(int projectNumber)
        {
            return await _context.Projects.SingleOrDefaultAsync(p => p.ProjectNumber == projectNumber);
        }

        public async Task UpdateProjectAsync(Project project)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task<Project[]> GetAllProjectsAsync()
        {
            return await _context.Projects.ToArrayAsync();
        }
        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<Project> GetProjectAsync(int projectID)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.ProjectID == projectID);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsWithStatusAsync()
        {
            return await _context.Projects.Include(p => p.Status).ToListAsync();
        }

        public async Task<Project> GetProjectWithStatusAsync(int projectId)
        {
            return await _context.Projects.Include(p => p.Status).FirstOrDefaultAsync(p => p.ProjectID == projectId);
        }

    }

}
