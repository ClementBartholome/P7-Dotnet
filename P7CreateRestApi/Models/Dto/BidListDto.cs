namespace P7CreateRestApi.Models.Dto;

public class BidListDto
{
    public int BidListId { get; set; }
    public string Account { get; set; }
    public string BidType { get; set; }
    public double? BidQuantity { get; set; }
}