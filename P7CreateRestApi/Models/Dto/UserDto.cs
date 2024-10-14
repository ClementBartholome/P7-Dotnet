using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models.Dto;

public class UserDto 
{
    public string Id { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "UserName cannot exceed 50 characters.")]
    public string UserName { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
    public string Password { get; set; }

    [StringLength(100, ErrorMessage = "FullName cannot exceed 100 characters.")]
    public string FullName { get; set; }

    [Required]
    public IList<string> Roles { get; set; }
    
}