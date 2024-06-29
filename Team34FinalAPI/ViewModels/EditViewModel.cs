using System.ComponentModel.DataAnnotations;

namespace Team34FinalAPI.ViewModels
{
    public class EditViewModel
    {
        /*[Key]
        [Required]
        public string UserName { get; set; }*/

        public string Name { get; set; }

        public string Surname { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        

    }
}
