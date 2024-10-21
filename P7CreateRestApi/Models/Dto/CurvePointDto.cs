using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models.Dto;

public class CurvePointDto
{
    public int Id { get; set; }

    [Required]
    public byte CurveId { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Term must be a non-negative number.")]
    public double? Term { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "CurvePointValue must be a non-negative number.")]
    public double? CurvePointValue { get; set; }
    
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public DateTime AsOfDate { get; set; } = DateTime.Now;


}