namespace Team34FinalAPI.ViewModels
{
    public class UpdateDetailsViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public IFormFile ProfilePhoto { get; set; } // For file uploads
    }
}
