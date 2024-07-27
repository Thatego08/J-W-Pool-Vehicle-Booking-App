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
        public bool ReminderSent { get; set; } // New property
       
    }
}
