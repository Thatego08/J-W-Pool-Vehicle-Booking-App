using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Team34FinalAPI.Models
{
    public class TripMedia
    {
        [Key]
        public int MediaId { get; set; }

        public int PostCheckId { get; set; }  // Foreign key to PostCheck

        [ForeignKey("PostCheckId")]
        [JsonIgnore]
        public PostCheck PostCheck { get; set; } // Navigation property

        public string Description { get; set; }
        public string? FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string? MediaType { get; set; }
    }
}
