using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Team34FinalAPI.Models
{
    public class RefuelVehicle
    {
        [Key]
        public int RefuelVehicleId { get; set; }

        [ForeignKey("Trip")]
       
        public int TripId { get; set; }



        public string OilLevel { get; set; }
       
      
       
   
        public string TyrePressure { get; set; }
        public string TyreCondition { get; set; }
       
        public string Comments { get; set; }

        public decimal FuelQuantity { get; set; }
        public decimal FuelCost { get; set; }

        [JsonIgnore]
        public Trip? Trip { get; set; }
    }
}
