using System.ComponentModel.DataAnnotations;

namespace Team34FinalAPI.ViewModels
{
    public class ResetPasswordViewModel
    {
      
        public string Email { get; set; }
        [Required]
        [StringLength(6, ErrorMessage = "The OTP must be 6 characters long.", MinimumLength = 6)]
        public string OTP { get; set; }
        public string NewPassword { get; set; }
/*
        [Required]
        public string Token { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(6, ErrorMessage = "The OTP must be 6 characters long.", MinimumLength = 6)]
        public string OTP { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
   */
    }
}
