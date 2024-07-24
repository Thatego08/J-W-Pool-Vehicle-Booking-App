namespace Team34FinalAPI.Models.CreateDto
{
    public class CreateRateDto
    {
        public int ProjectNumber { get; set; }
        public string TypeName { get; set; }
        public decimal RateValue { get; set; }
        public string ApplicableTimePeriod { get; set; }
        public string Conditions { get; set; }

    }
}
