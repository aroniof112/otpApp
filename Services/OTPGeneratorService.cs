using Newtonsoft.Json;
using OTPApplication.Interfaces;

public class OTPGeneratorService : IOTPGeneratorService
{
    private readonly ISaveUserToFile _saveUserToFile;
    private readonly IHashPasswordService _hashPasswordService;
    private readonly IUserRepository _userRepository;
    private readonly TimeSpan PasswordDuration = TimeSpan.FromSeconds(30);

    public OTPGeneratorService(ISaveUserToFile saveUserToFile, IHashPasswordService hashPasswordService, 
        IUserRepository userRepository)
    {
        _saveUserToFile = saveUserToFile;
        _hashPasswordService = hashPasswordService;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Generating a new password for the user that is valid 30s.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>A string representing the new generated password</returns>
    public async Task<string> GeneratePassword(int userId)
    {
        DateTime currentTime = DateTime.Now;

        // Get the user Json from file
        string passwordAndTime = await _saveUserToFile.GetLastSavedUser();
        User data = ExtractPasswordAndTimeFromJson(passwordAndTime);

        // Verify if user exists
        var userRepo = await _userRepository.GetUser(userId);
        if (userRepo == null) return "User not found";

        if (data == null || string.IsNullOrEmpty(data.Password))
        {
            data = InitializeDefaultUser(userRepo);
        }

        string hashedBytes = data.Password;
        DateTime lastGeneratedTime = data.LastGeneratedTime;      
        
        // Check if the current password is still valid
        if (hashedBytes != null && (currentTime - lastGeneratedTime) <= PasswordDuration && data.Id == userId)
        {
            return hashedBytes;
        }

        //Generate a new password
        string currentPassword = userRepo.Id + "-" + currentTime.ToString("ddMMyyyymmss");
        lastGeneratedTime = currentTime;
        hashedBytes = await _hashPasswordService.HashPassword(currentPassword);

        // Save the new hashed password and last generated time to file
        userRepo.Password = hashedBytes;
        userRepo.LastGeneratedTime = lastGeneratedTime;

        // Serialize the User object to JSON
        string userJson = JsonConvert.SerializeObject(userRepo);
        await _saveUserToFile.SaveUser(userJson);

        userRepo.LastGeneratedTime = lastGeneratedTime;

        return hashedBytes;
    }

    /// <summary>
    /// Check if the password is valid.
    /// </summary>
    /// <param name="password"></param>
    /// <returns>A bool that is true when password is valid and false when not valid.</returns>
    public async Task<bool> IsPasswordValid(string password)
    {
        // Get the user Json from file
        string passwordAndTime = await _saveUserToFile.GetLastSavedUser();
        User data = ExtractPasswordAndTimeFromJson(passwordAndTime);
        if (passwordAndTime == "") return false;

        var currentPass = data.Password;
        DateTime lastGeneratedTime = data.LastGeneratedTime;

        // Check if the password matches the current password
        if (password == currentPass)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan elapsed = currentTime - lastGeneratedTime;
            return elapsed <= PasswordDuration;
        }

        return false;
    }

    private User ExtractPasswordAndTimeFromJson(string json)
    {
        // Deserialize the JSON into a PasswordAndTimeData object
        User data = JsonConvert.DeserializeObject<User>(json);
        return data;
    }

    private User InitializeDefaultUser(User user)
    {
        // Initialize a default User object
        User defaultUser = new User
        {
            Id = user.Id,
            Password = "", // or set a default password
            LastGeneratedTime = DateTime.MinValue // or set a default value
        };

        return defaultUser;
    }
}
