using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Controllers
{

    [Authorize(Roles = "Admin,Driver")]
    [ApiController]
    [Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;

    private readonly ILogger<ProjectController> _logger;
        private readonly IRateRepo _rateRepository; // Injecting rate repository


        public ProjectController(IProjectRepository projectRepository, IRateRepo rateRepository, ILogger<ProjectController> logger)
    {
            _rateRepository = rateRepository ?? throw new ArgumentNullException(nameof(rateRepository)); // Initialize _rateRepository

            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost]
    [Route("AddProject")]
    public async Task<IActionResult> AddProject([FromBody] ProjectViewModel pvm)
    {
            try
            {
                if (pvm == null)
                {
                    _logger.LogWarning("ProjectViewModel is null.");
                    return BadRequest("Project model is null.");
                }

                var project = new Project
                {
                    ProjectNumber = pvm.ProjectNumber,
                    JobNo = pvm.JobNo,
                    Description = pvm.Description,
                    TaskCode = pvm.TaskCode,
                    ActivityCode = pvm.ActivityCode,
                    StatusId = 1 //Default to availabe status
                };
               

                _logger.LogInformation("Adding project: {@Project}", project);

                await _projectRepository.AddProjectAsync(project);
                await _projectRepository.SaveChangesAsync();

                _logger.LogInformation("Project added successfully with ID: {ProjectID}", project.ProjectID);
                return Ok("Project added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding project");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    [HttpPut]
    [Route("EditProject/{ProjectID}")]
    public async Task<IActionResult> EditProject(int ProjectID, [FromBody] ProjectViewModel pvm)
    {
        if (pvm == null)
        {
            return BadRequest("Project model is null.");
        }

        try
        {
            var existingProject = await _projectRepository.GetProjectAsync(ProjectID);
            if (existingProject == null)
            {
                return NotFound("Project does not exist.");
            }
            existingProject.ProjectID = pvm.ProjectID;
            existingProject.ProjectNumber = pvm.ProjectNumber;
            existingProject.Description = pvm.Description;
            existingProject.JobNo = pvm.JobNo;
            existingProject.TaskCode = pvm.TaskCode;
            existingProject.ActivityCode = pvm.ActivityCode;

            await _projectRepository.UpdateProjectAsync(existingProject);

            return Ok(existingProject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while editing the project.");
            return StatusCode(500, "Internal server error, contact support.");
        }
    }

        [HttpGet]
        [Route("projectNumbers")]
        public async Task<ActionResult<IEnumerable<int>>> GetProjectNumbers()
        {
            try
            {
                var projects = await _projectRepository.GetProjectsAsync();
                var projectNumbers = projects.Select(p => p.ProjectNumber).ToList();
                return Ok(projectNumbers);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet]
    [Route("GetAllProjects")]
    public async Task<IActionResult> GetAllProjects()
    {
        try
        {
            var results = await _projectRepository.GetAllProjectsAsync();
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving projects.");
            return StatusCode(500, "Internal server error, contact support.");
        }
    }

    [HttpGet]
    [Route("GetProject/{ProjectID}")]
    public async Task<IActionResult> GetProject(int ProjectID)
    {
        try
        {
            var result = await _projectRepository.GetProjectAsync(ProjectID);

            if (result == null)
            {
                return NotFound("Project does not exist.");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the project.");
            return StatusCode(500, "Internal server error, contact support.");
        }
    }

    [HttpDelete]
    [Route("DeleteProject/{ProjectID}")]
    public async Task<IActionResult> DeleteProject(int ProjectID)
    {
        try
        {
            var existingProject = await _projectRepository.GetProjectAsync(ProjectID);

            if (existingProject == null)
            {
                return NotFound("Project does not exist.");
            }

            _projectRepository.Delete(existingProject);

            if (await _projectRepository.SaveChangesAsync())
            {
                return Ok(existingProject);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the project.");
            return StatusCode(500, "Internal server error, contact support.");
        }

        return BadRequest("Your request is invalid.");
    }
}

}
