using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models.Dto;

public class RuleDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; }

    [Required]
    public string Json { get; set; }

    [Required]
    public string Template { get; set; }

    [Required]
    public string SqlStr { get; set; }

    [Required]
    public string SqlPart { get; set; }
}