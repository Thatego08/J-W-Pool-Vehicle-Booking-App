namespace Team34FinalAPI.Services
{
    public interface ISMS_Service
    {
        Task SendSmsAsync(string toPhoneNumber, string message);

    }
}
