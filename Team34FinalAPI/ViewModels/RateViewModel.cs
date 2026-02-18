namespace Team34FinalAPI.ViewModels
{
    public class RateViewModel
    {/*
        public int RateTypeID { get; set; }  // For creating/updating a rate
        public string RateTypeName { get; set; }
     */
        public int RateID { get; set; }

        public int  ProjectID { get; set; }
        public decimal RateValue { get; set; }
        
        /*public List<int> ProjectNumbers { get; set; }  */// Updated to handle multiple projects
        
        public string? ApplicableTimePeriod { get; set; }
        public string? Conditions { get; set; }

    }
}
