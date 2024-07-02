namespace Team34FinalAPI.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int JobNo { get; set; }
        public int TaskCode { get; set; }
        public int ActivityCode { get; set; }


        public decimal HalfDayRate { get; set; } 
        public decimal FullDayRate { get; set; }
        // This line establishes the one-to-many relationship
        public ICollection<Booking> Bookings { get; set; }
    }
}
