namespace Team34FinalAPI.Models
{
    public class ProjectRate
    {
        
        public int ProjectID { get; set; }
        public Project Project { get; set; }

        public int RateID { get; set; }
        public Rate Rate { get; set; }
    }
}
