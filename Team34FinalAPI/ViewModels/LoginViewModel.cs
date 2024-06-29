using System.ComponentModel.DataAnnotations;

namespace Team34FinalAPI.ViewModels
{
    public class LoginViewModel
    {
        [Key]
        [Required]
        public string UserName { get; set; }

        /*public LoginViewModel(string userName)
        {
            User. = userName;
        }*/

        [Required]
        public string Password { get; set; }
    }
}
