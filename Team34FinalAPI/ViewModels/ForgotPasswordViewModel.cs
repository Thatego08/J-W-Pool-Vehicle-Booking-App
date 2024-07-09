using System.ComponentModel.DataAnnotations;

namespace Team34FinalAPI.ViewModels
{
    public class ForgotPasswordViewModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
