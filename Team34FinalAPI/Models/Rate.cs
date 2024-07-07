using System.ComponentModel.DataAnnotations;

namespace Team34FinalAPI.Models
{
    public class Rate
    {
         
        [Key]
        public int RateId { get; set; }
        public decimal HalfDayRate { get; set; }
        public decimal FullDayRate { get; set; }
        public decimal KilometreRate { get; set; }
    }

}

