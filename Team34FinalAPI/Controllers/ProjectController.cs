using iText.Kernel.Counter.Context;
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
        private readonly IStatusRepository _statusRepository;  // Added status repository for status lookups
        private readonly ILogger<ProjectController> _logger;
        private readonly RateEEDBContext _context;

        public ProjectController(IProjectRepository projectRepository, IStatusRepository statusRepository, ILogger<ProjectController> logger, RateEEDBContext context)
        {
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _statusRepository = statusRepository ?? throw new ArgumentNullException(nameof(statusRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context;
        }

        // POST: Add a new project
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

                // Validate StatusId
                var status = await _statusRepository.GetStatusByIdAsync(pvm.StatusId ?? 1); // Default to 1 if null
                if (status == null)
                {
                    return BadRequest("Invalid Status ID.");
                }

                var project = new Project
                {
                    ProjectNumber = pvm.ProjectNumber,
                    JobNo = pvm.JobNo,
                    Description = pvm.Description,
                    TaskCode = pvm.TaskCode,
                    ActivityCode = pvm.ActivityCode,
                    StatusId = pvm.StatusId ?? 1 // Use the validated StatusId
                };

                _logger.LogInformation("Adding project: {@Project}", project);

                await _projectRepository.AddProjectAsync(project);
                await _projectRepository.SaveChangesAsync();

                // If RateID list is provided, update those rates
                if (pvm.RateID != null && pvm.RateID.Any())
                {
                    var rates = await _context.RatesEE
                        .Where(r => pvm.RateID.Contains(r.RateId))
                        .ToListAsync();
                    foreach (var rate in rates)
                    {
                        rate.ProjectId = project.ProjectID;
                    }
                    await _context.SaveChangesAsync();
                }

                _logger.LogInformation("Project added successfully with ID: {ProjectID}", project.ProjectID);
                return Ok(new { message = "Project added successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding project");
                return StatusCode(500, $"Internal server error: {ex.Message}");
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

        // PUT: Edit an existing project
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

                // Validate StatusId if provided
                if (pvm.StatusId.HasValue)
                {
                    var status = await _statusRepository.GetStatusByIdAsync(pvm.StatusId.Value);
                    if (status == null)
                    {
                        return BadRequest("Invalid Status ID.");
                    }
                    existingProject.StatusId = pvm.StatusId.Value;  // Update status
                }

                // Update other fields
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

        // GET: Get all projects with status names
        [HttpGet]
        [Route("GetAllProjects")]
        public async Task<IActionResult> GetAllProjects()
        {
            try
            {
                var projects = await _projectRepository.GetAllProjectsWithStatusAsync(); // Modified to include status
                var projectViewModels = projects.Select(p => new ProjectViewModel
                {
                    ProjectID = p.ProjectID,
                    ProjectNumber = p.ProjectNumber,
                    JobNo = p.JobNo,
                    TaskCode = p.TaskCode,
                    Description = p.Description,
                    ActivityCode = p.ActivityCode,
                    StatusId = p.StatusId,
                    StatusName = p.Status.Name, // Include status name in response
                    RateID = p.RatesEE?.Select(r => r.RateId).ToList() ?? new List<int>() // ensures not null
                }).ToList();
            

                return Ok(projectViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving projects.");
                return StatusCode(500, "Internal server error, contact support.");
            }
        }

        // GET: Get a single project by ID with status name
        [HttpGet]
        [Route("GetProject/{ProjectID}")]
        public async Task<IActionResult> GetProject(int ProjectID)
        {
            try
            {
                var project = await _projectRepository.GetProjectWithStatusAsync(ProjectID); // Modified to include status
                if (project == null)
                {
                    return NotFound("Project does not exist.");
                }

                var projectViewModel = new ProjectViewModel
                {
                    ProjectID = project.ProjectID,
                    ProjectNumber = project.ProjectNumber,
                    JobNo = project.JobNo,
                    TaskCode = project.TaskCode,
                    Description = project.Description,
                    ActivityCode = project.ActivityCode,
                    StatusId = project.StatusId,
                    StatusName = project.Status.Name // Include status name
                };

                return Ok(projectViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the project.");
                return StatusCode(500, "Internal server error, contact support.");
            }
        }

        // DELETE: Delete a project by ID
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
