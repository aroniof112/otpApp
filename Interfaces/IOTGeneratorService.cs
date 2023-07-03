namespace OTPApplication.Interfaces
{
    public interface IOTPGeneratorService
    {
        Task<string> GeneratePassword(int userId);
        Task<bool> IsPasswordValid(string password);
    }
}
