using Bogus;
using OTPApplication.Interfaces;

namespace OTPApplication.Data
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> GetUser(int userId)
        {
            List<User> fakeUsers = GenerateFakeUsers();

            User user = fakeUsers.FirstOrDefault(u => u.Id == userId);

            return user;
        }

        private List<User> GenerateFakeUsers()
        {
            var userList = new List<User>
            {
                new User { Id = 1, Password = "", LastGeneratedTime = DateTime.MinValue },
                new User { Id = 2, Password = "", LastGeneratedTime = DateTime.MinValue },
                new User { Id = 3, Password = "", LastGeneratedTime = DateTime.MinValue }
            };

            return userList;
        }
    }
}