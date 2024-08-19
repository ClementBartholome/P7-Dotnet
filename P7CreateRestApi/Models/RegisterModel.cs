using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models;

public class RegisterModel
{
    public RegisterModel(string fullName, string email, string password)
    {
        FullName = fullName;
        Email = email;
        Password = password;
    }

    [Required]
    [StringLength(100, ErrorMessage = "FullName cannot exceed 100 characters.")]
    public string FullName { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
    public string Password { get; set; }
}