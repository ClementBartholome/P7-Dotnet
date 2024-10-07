using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models.Dto;

public class RatingDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "MoodysRating cannot exceed 50 characters.")]
    public string MoodysRating { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "SandPRating cannot exceed 50 characters.")]
    public string SandPRating { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "FitchRating cannot exceed 50 characters.")]
    public string FitchRating { get; set; }

    [Range(0, byte.MaxValue, ErrorMessage = "OrderNumber must be a non-negative number.")]
    public byte? OrderNumber { get; set; }
}