namespace Team34FinalAPI.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        public int ProjectNumber { get; set; }

        public int JobNo { get; set; }
        public int TaskCode { get; set; }
        public string Description { get; set; }
        public int ActivityCode { get; set; }

        public int? StatusId { get; set; }
        public Status? Status { get; set; }
        // This line establishes the one-to-many relationship
        public ICollection<Booking> Bookings { get; set; }
        // Many-to-many relationship with Rate
        public ICollection<ProjectRate> ProjectRates { get; set; }
    }
}
