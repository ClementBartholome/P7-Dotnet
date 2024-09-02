using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models.Dto;

public class UserDto
{
    public string Id { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "UserName cannot exceed 50 characters.")]
    public string UserName { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
    public string Password { get; set; }

    [StringLength(100, ErrorMessage = "FullName cannot exceed 100 characters.")]
    public string FullName { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
    public IList<string> Roles { get; set; }
}