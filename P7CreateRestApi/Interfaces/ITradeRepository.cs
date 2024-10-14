using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Models.Dto;

namespace P7CreateRestApi.Repositories;

public interface ITradeRepository
{
    Task<ActionResult<IEnumerable<TradeDto>>> GetTrades();
    Task<TradeDto?> GetTrade(int id);
    Task<Trade?> UpdateTrade(int id, TradeDto tradeDto);
    Task<Trade> PostTrade(TradeDto tradeDto);
    Task<bool> DeleteTrade(int id);
}