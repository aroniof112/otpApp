using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using OTPApplication.Interfaces;

namespace OTPApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class OTPController : BaseApiController
{
    private readonly IOTPGeneratorService _passwordGenerator;
    public OTPController(IOTPGeneratorService passwordGenerator)
    {
        _passwordGenerator = passwordGenerator;
    }

    /// <summary>
    /// Generates the unique password for the user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>A string representing the password</returns>
    [HttpGet("password")]
    public async Task<ActionResult<string>> GetPassword(int userId)
    {
        var password = await _passwordGenerator.GeneratePassword(userId);

        return Ok(password);
    }

    /// <summary>
    /// Check if the password is valid
    /// </summary>
    /// <param name="password"></param>
    /// <returns>True if password is valid, False if otherwise</returns>
    [HttpGet("checkPassword")]
    public async Task<ActionResult<bool>> VerifyPassword(string password)
    {
        var verify = await _passwordGenerator.IsPasswordValid(password);

        return Ok(verify);
    }
}
