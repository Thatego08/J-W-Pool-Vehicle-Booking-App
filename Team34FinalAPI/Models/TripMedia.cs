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
        public Trip Trip { get; set; }

        public string MediaPath { get; set; }

        public string Description { get; set; }

        // Additional properties for file storage
        public string FileName { get; set; }
        public byte[] FileContent { get; set; } // Store file content as byte array
        public string MediaType { get; set; } // MIME type of the file

    }
}
