using System.ComponentModel.DataAnnotations;

namespace Team34FinalAPI.ViewModels
{
    public class RatesViewModelEE
    {

        public class CreateRateDto
        {
            [Required]
            [StringLength(100)]
            public string RateName { get; set; }

            [Required]
            [Range(0, double.MaxValue)]
            public decimal RateValue { get; set; }

            public DateTime? EffectiveDate { get; set; }
            public DateTime? ExpiryDate { get; set; }

            public bool IsActive { get; set; } = true;

            [Required]
            public int ProjectId { get; set; }
        }

        public class UpdateRateDto : CreateRateDto
        {
            [Required]
            public int RateId { get; set; }
        }

        public class RateResponseDto
        {
            public int RateId { get; set; }
            public string RateName { get; set; }
            public decimal RateValue { get; set; }
            public DateTime? EffectiveDate { get; set; }
            public DateTime? ExpiryDate { get; set; }
            public bool IsActive { get; set; }
            public int ProjectId { get; set; }
            public int ProjectNumber { get; set; }
            public string Description { get; set; }  // e.g., project number or description
            public DateTime CreatedAt { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public string UpdatedBy { get; set; }
        }
    }
}
