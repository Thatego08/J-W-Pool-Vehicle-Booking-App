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
        public decimal HalfDayRate { get; set; } // New property
        public decimal FullDayRate { get; set; } // New property

        // This line establishes the one-to-many relationship
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Rate> Rates { get; set; }
    }
}
