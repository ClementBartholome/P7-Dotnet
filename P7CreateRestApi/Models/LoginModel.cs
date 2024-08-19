using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models;

public class LoginModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}