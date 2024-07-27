namespace Team34FinalAPI.ViewModels
{
    public class RateViewModel
    {
        public int RateTypeID { get; set; }
        public decimal RateValue { get; set; }
        public int ProjectID { get; set; }
        public string? ApplicableTimePeriod { get; set; }
        public string? Conditions { get; set; }
        
    }
}
