using OTPApplication.Interfaces;

namespace OTPApplication.Services
{
    public class SaveUserToFile : ISaveUserToFile
    {
        private const string FilePathUser = "user.txt";

        public async Task<string> GetLastSavedUser()
        {
            return await ReadUserFromFile();
        }

        public async Task<string> ReadUserFromFile()
        {
            try
            {
                if (File.Exists(FilePathUser))
                {
                    return await File.ReadAllTextAsync(FilePathUser);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading password file: {ex.Message}");
            }

            return null;
        }

        public async Task SaveUser(string password)
        {
            await File.WriteAllTextAsync(FilePathUser, password);
        }
    }
}
