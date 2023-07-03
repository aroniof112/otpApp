namespace OTPApplication.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUser(int userId);
    }
}
