namespace Team34FinalAPI.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public int Rating { get; set; }
    }
}
