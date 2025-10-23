namespace Team34FinalAPI.ViewModels
{
    public class UserViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public string ConfirmPassword { get; set; }
        // Role property removed - automatically set to "Driver"
    }


}
