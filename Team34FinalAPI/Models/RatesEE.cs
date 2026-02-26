using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Team34FinalAPI.Models
{
    [Table ("RatesEE")]
    public class RatesEE
    {
        [Key]
        public int RateId { get; set; }

        [Required]
        [StringLength(100)]
        public string RateName { get; set; }          // e.g., "Hourly Rate", "Daily Rate"

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RateValue { get; set; }

        public DateTime? EffectiveDate { get; set; }   // When this rate becomes active
        public DateTime? ExpiryDate { get; set; }      // Optional expiration

        public bool IsActive { get; set; } = true;

        // Foreign key to Project
        [Required]
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        // Audit fields (optional)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

    }
}
