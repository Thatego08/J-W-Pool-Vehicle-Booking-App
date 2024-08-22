using System.ComponentModel.DataAnnotations;

namespace Team34FinalAPI.ViewModels
{
    public class VerifyOTPViewModel
    {
        [Required]
        public string OTP { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
