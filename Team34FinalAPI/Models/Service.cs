using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Team34FinalAPI.Models
{
    public class Service
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
