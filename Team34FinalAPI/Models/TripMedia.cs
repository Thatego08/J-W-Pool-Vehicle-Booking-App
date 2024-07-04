using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team34FinalAPI.Models
{
    public class TripMedia
    {

        [Key]
        public int MediaId { get; set; }

        public int TripId { get; set; }

        [ForeignKey("TripId")]
        public Trip Trip { get; set; } // Navigation property
        public string Description { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string MediaType { get; set; }


    }
}
