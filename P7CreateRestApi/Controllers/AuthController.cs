using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Services;

namespace P7CreateRestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly JwtService _jwtService;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, JwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        var user = new User { UserName = model.Email, FullName = model.FullName, Email = model.Email, EmailConfirmed = true };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            return Ok(new { message = "Utilisateur enregistré avec succès" });
        }
        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var token = _jwtService.GenerateJwtToken(user);
            return Ok(new { token });
        }
        return Unauthorized();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(new { message = "Utilisateur déconnecté avec succès" });
    }
}