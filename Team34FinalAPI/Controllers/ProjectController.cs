using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Controllers
{

    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<ProjectController> _logger;

    public ProjectController(IProjectRepository projectRepository, ILogger<ProjectController> logger)
    {
        _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost]
    [Route("AddProject")]
    public async Task<IActionResult> AddProject([FromBody] ProjectViewModel pvm)
    {
        if (pvm == null)
        {
            return BadRequest("Project model is null.");
        }

        var project = new Project
        {
            ProjectID = pvm.ProjectID,
            ProjectNumber = pvm.ProjectNumber,
            JobNo = pvm.JobNo,
            TaskCode = pvm.TaskCode,
            ActivityCode = pvm.ActivityCode,
          
        };

        try
        {
            await _projectRepository.AddProjectAsync(project);
                await _projectRepository.SaveChangesAsync();
            return Ok("Project added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding the project.");
            return StatusCode(500, "Internal server error, contact support.");
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
