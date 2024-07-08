using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team34FinalAPI.Models
{
    public class Rate
    {

        [Key]
        public int RateId { get; set; }

        [Required]
        public string RateType { get; set; }  // "Kilometer", "HalfDay", "FullDay"

        [Required]
        public decimal RateValue { get; set; }

        [Required]
        public int ProjectId { get; set; }

        // Navigation property to Project
        public Project Project { get; set; }
    }

}

