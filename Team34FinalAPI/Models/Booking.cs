using System.ComponentModel.DataAnnotations;

namespace Team34FinalAPI.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        [Required]
        public string UserName { get; set; }
        
        public string? Event { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int VehicleId { get; set; }
        [Required]
        public Vehicle Vehicle { get; set; }

        public int? ProjectId { get; set; }
        
        public Project? Project { get; set; }

        public string Type { get; set; }
        public bool ReminderSent { get; set; } // New property

        public int? StatusId { get; set; }
        public Status? Status { get; set; }
        public ICollection<Trip> Trips { get; set; }
        public ICollection<PreChecklist> PreChecklists { get; set; } = new List<PreChecklist>();

    }
}
