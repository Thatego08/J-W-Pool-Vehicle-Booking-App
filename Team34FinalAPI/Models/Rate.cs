using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team34FinalAPI.Models;

public class Rate
{

    [Key]
    public int RateID { get; set; }



    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal RateValue { get; set; }

    [Required]
    [ForeignKey("ProjectID")]
    public int ProjectID { get; set; }

    public Project Project { get; set; }

    // Foreign key to RateType (optional)
    public int? RateTypeID { get; set; }
    public RateType RateType { get; set; }

    public string? ApplicableTimePeriod { get; set; }

    public string? Conditions { get; set; }

}

