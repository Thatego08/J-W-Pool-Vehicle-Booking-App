using System.ComponentModel.DataAnnotations;

namespace Team34FinalAPI.Models
{
    public class InspectionList
    {
        [Key]
        public int ChecklistID { get; set; }
        public string Item { get; set; }
        public bool IsCompleted { get; set; }
    }
}
