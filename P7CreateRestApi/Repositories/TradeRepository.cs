using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Data;

namespace P7CreateRestApi.Repositories
{
    public class TradeRepository
    {
        private readonly LocalDbContext _context;

        public TradeRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<TradeDto>>> GetTrades()
        {
            return await _context.Trades
                .Select(trade => new TradeDto
                {
                    TradeId = trade.TradeId,
                    Account = trade.Account,
                    AccountType = trade.AccountType,
                    BuyQuantity = trade.BuyQuantity,
                    SellQuantity = trade.SellQuantity,
                    BuyPrice = trade.BuyPrice,
                    SellPrice = trade.SellPrice,
                    TradeDate = trade.TradeDate,
                    TradeSecurity = trade.TradeSecurity,
                    TradeStatus = trade.TradeStatus,
                    Trader = trade.Trader,
                    Benchmark = trade.Benchmark,
                    Book = trade.Book,
                    CreationName = trade.CreationName,
                    RevisionName = trade.RevisionName,
                    DealName = trade.DealName
                })
                .ToListAsync();
        }

        public async Task<TradeDto?> GetTrade(int id)
        {
            var trade = await _context.Trades.FindAsync(id);

            if (trade == null)
            {
                return null;
            }

            return new TradeDto
            {
                TradeId = trade.TradeId,
                Account = trade.Account,
                AccountType = trade.AccountType,
                BuyQuantity = trade.BuyQuantity,
                SellQuantity = trade.SellQuantity,
                BuyPrice = trade.BuyPrice,
                SellPrice = trade.SellPrice,
                TradeDate = trade.TradeDate,
                TradeSecurity = trade.TradeSecurity,
                TradeStatus = trade.TradeStatus,
                Trader = trade.Trader,
                Benchmark = trade.Benchmark,
                Book = trade.Book,
                CreationName = trade.CreationName,
                RevisionName = trade.RevisionName,
                DealName = trade.DealName
            };
        }

        public async Task<Trade?> UpdateTrade(int id, TradeDto tradeDto)
        {
            var trade = await _context.Trades.FindAsync(id);

            if (trade == null)
            {
                return null;
            }

            trade.Account = tradeDto.Account;
            trade.AccountType = tradeDto.AccountType;
            trade.BuyQuantity = tradeDto.BuyQuantity;
            trade.SellQuantity = tradeDto.SellQuantity;
            trade.BuyPrice = tradeDto.BuyPrice;
            trade.SellPrice = tradeDto.SellPrice;
            trade.TradeDate = tradeDto.TradeDate;
            trade.TradeSecurity = tradeDto.TradeSecurity;
            trade.TradeStatus = tradeDto.TradeStatus;
            trade.Trader = tradeDto.Trader;
            trade.Benchmark = tradeDto.Benchmark;
            trade.Book = tradeDto.Book;
            trade.CreationName = tradeDto.CreationName;
            trade.RevisionName = tradeDto.RevisionName;
            trade.DealName = tradeDto.DealName;

            _context.Set<Trade>().Update(trade);
            await _context.SaveChangesAsync();
            return trade;
        }

        public async Task<Trade> PostTrade(TradeDto tradeDto)
        {
            var trade = new Trade
            {
                Account = tradeDto.Account,
                AccountType = tradeDto.AccountType,
                BuyQuantity = tradeDto.BuyQuantity,
                SellQuantity = tradeDto.SellQuantity,
                BuyPrice = tradeDto.BuyPrice,
                SellPrice = tradeDto.SellPrice,
                TradeDate = tradeDto.TradeDate,
                TradeSecurity = tradeDto.TradeSecurity,
                TradeStatus = tradeDto.TradeStatus,
                Trader = tradeDto.Trader,
                Benchmark = tradeDto.Benchmark,
                Book = tradeDto.Book,
                CreationName = tradeDto.CreationName,
                RevisionName = tradeDto.RevisionName,
                DealName = tradeDto.DealName
            };

            _context.Trades.Add(trade);
            await _context.SaveChangesAsync();
            return trade;
        }

        public async Task<bool> DeleteTrade(int id)
        {
            var trade = await _context.Trades.FindAsync(id);
            
            if (trade == null)
            {
                return false;
            }

            _context.Trades.Remove(trade);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}