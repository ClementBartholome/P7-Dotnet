using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace P7CreateRestApi.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IJwtService jwtService,
        ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _logger = logger;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        _logger.LogInformation("Registering User");
        var user = new User
            { UserName = model.Email, FullName = model.FullName, Email = model.Email, EmailConfirmed = true };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
            _logger.LogInformation("User registered successfully with email: {Email}", model.Email);
            return Ok(new { message = "User registered successfully" });
        }
        _logger.LogWarning("User registration failed for email: {Email}", model.Email);
        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [SwaggerOperation(
        Description =
            "Admin credentials => Email: admin@example.com, Password: Password123$. Normal user credentials => Email: user@example.com, Password: Password123$"
    )]
    public async Task<IActionResult> Login(LoginModel model)
    {
        _logger.LogInformation("Login method called with email: {Email}", model.Email);
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var token = _jwtService.GenerateJwtToken(user);
            _logger.LogInformation("User logged in successfully with email: {Email}", model.Email);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // empêche les scripts côté client d'accéder au cookie
                Secure = true, // le cookie ne sera envoyé que sur des connexions HTTPS
                SameSite = SameSiteMode
                    .Strict, // empêche le navigateur d'envoyer le cookie avec des requêtes de site croisé
                Expires = DateTime.UtcNow.AddHours(1)
            };
            Response.Cookies.Append("jwt", token, cookieOptions);

            return Ok(new { message = "Login successful" });
        }

        _logger.LogWarning("Login failed for email: {Email}", model.Email);
        return Unauthorized();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        _logger.LogInformation("Logout method called");
        await _signInManager.SignOutAsync();

        Response.Cookies.Delete("jwt");

        _logger.LogInformation("User logged out successfully");
        return Ok(new { message = "User logged out successfully" });
    }
}