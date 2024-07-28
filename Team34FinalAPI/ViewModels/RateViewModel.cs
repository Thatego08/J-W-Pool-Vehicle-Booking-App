namespace Team34FinalAPI.ViewModels
{
    public class RateViewModel
    {
        public string RateTypeName { get; set; }
        public decimal RateValue { get; set; }
        public int ProjectNumber { get; set; }
        public string? ApplicableTimePeriod { get; set; }
        public string? Conditions { get; set; }

    }
}
