namespace OTPApplication.Interfaces
{
    public interface IHashPasswordService
    {
        Task<string> HashPassword(string password);
    }
}
