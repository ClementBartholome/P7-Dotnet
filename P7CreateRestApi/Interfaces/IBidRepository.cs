using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace P7CreateRestApi.Interfaces;

public interface IBidRepository
{
    Task<ActionResult<IEnumerable<BidListDto>>> GetBidLists();
    Task<BidListDto> GetBidList(int id);
    Task<BidList?> UpdateBidList(int id, BidListDto bidListDto);
    Task<BidList> PostBidList(BidListDto bidListDto);
    Task DeleteBidList(int id);
}