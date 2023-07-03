namespace OTPApplication.Interfaces
{
    public interface ISaveUserToFile
    {
        Task SaveUser(string password);
        Task<string> ReadUserFromFile();
        Task<string> GetLastSavedUser();
    }
}
