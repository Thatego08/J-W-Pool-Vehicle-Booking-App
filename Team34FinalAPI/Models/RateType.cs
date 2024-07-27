using System.ComponentModel.DataAnnotations;

namespace Team34FinalAPI.Models
{
    public class RateType
    {
        [Key]
        public int RateTypeID { get; set; }
        public string TypeName { get; set; }

        public ICollection<Rate> Rates { get; set; }
    }
}
