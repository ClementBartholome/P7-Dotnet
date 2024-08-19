using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models.Dto;

public class CurvePointDto
{
    public int Id { get; set; }

    [Required]
    public byte? CurveId { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Term must be a non-negative number.")]
    public double? Term { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "CurvePointValue must be a non-negative number.")]
    public double? CurvePointValue { get; set; }
}