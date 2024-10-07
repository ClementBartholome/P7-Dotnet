using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models.Dto;

public class BidListDto
{
    public int BidListId { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Account name cannot exceed 50 characters.")]
    public string Account { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "BidType cannot exceed 50 characters.")]
    public string BidType { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "BidQuantity must be a non-negative number.")]
    public double? BidQuantity { get; set; }
}