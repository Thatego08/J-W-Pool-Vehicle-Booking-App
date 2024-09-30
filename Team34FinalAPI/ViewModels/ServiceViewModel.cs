using System.ComponentModel.DataAnnotations;

namespace Team34FinalAPI.ViewModels
{
    public class ServiceViewModel
    {
        
        public int ServiceID { get; set; }

        public int VehicleID { get; set; }

        public string AdminName { get; set; }

        [EmailAddress]
        public string AdminEmail { get; set; }

        public string Description { get; set; } //Additional Details

        public DateTime ServiceDate { get; set; }
    }
}
