namespace Team34FinalAPI.Models
{
    public class FAQ
    {
        public int FAQId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool IsPostedToWebsite { get; set; }
    }
}
