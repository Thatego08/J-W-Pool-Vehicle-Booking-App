using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        // Initialize Repo 
        private readonly IAdminRepo _adminRepo;

        public AdminController(IAdminRepo adminRepo, IConfiguration configuration)
        {
            _adminRepo = adminRepo;

        }

        [HttpGet]
        [Route("getAllAdmins")]
        public async Task<IActionResult> GetAllAdmin()
        {
            try
            {
                return Ok(await _adminRepo.GetAllAdminAsync());
            }
            catch (Exception ex)
            {
                // Notify  Admins
                return BadRequest("An Error Occoured , Please Try Again Later");
            }
        }


        [HttpGet]
        [Route("getAllAdmins/{UserName}")]
        public async Task<IActionResult> GetAdmin(string UserName)
        {
            try
            {
                var admin = await _adminRepo.GetAdmin(UserName);
                if (admin == null)
                {
                    return NotFound("Not able to locate admin");
                }
                return Ok(admin);
            }
            catch (Exception ex)
            {
                // Notify  Admins
                return BadRequest("An Error Occoured , Please Try Again Later");
            }
        }


        [HttpPost]
        [Route("addAmin")]
        public async Task<IActionResult> AddAdmin([FromBody] User user)
        {
            try
            {
                var message = "Successfully Added Admin User";
                var added = await _adminRepo.AddAdmin(user);

                if (!added)
                {
                    message = "Received Data but Failed to Add , Please check the time idk";
                }
                return Ok(message);
            }
            catch (Exception ex)
            {
                // Notify  Admins
                return BadRequest("An Error Occoured , Please Try Again Later");
            }
        }

        [HttpPost]
        [Route("updateAdmin")]
        public async Task<IActionResult> UpdateAdmin([FromBody] User user)
        {
            try
            {
                var message = "Successfully Updated Admin User";
                var added = await _adminRepo.UpdateAdmin(user);
                if (!added)
                {
                    message = "Received Data but Failed to Update, Please check the time idk";
                }
                return Ok(message);
            }
            catch (Exception ex)
            {
                // Notify user
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


        [HttpGet]
        [Route("deleteAdmin/{UserName}")]
        public async Task<IActionResult> DeleteAdmin(string UserName)
        {
            try
            {
                var message = "Successfully Removed Admin";
                var added = await _adminRepo.DeleteAdmin(UserName);
                if (!added)
                {
                    message = "Received Data but Failed to Delete";
                }
                return Ok(message);
            }
            catch (Exception ex)
            {
                // Notify user
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


    
}
}
