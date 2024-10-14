using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Data;
using P7CreateRestApi.Interfaces;
using P7CreateRestApi.Models.Dto;


namespace P7CreateRestApi.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly LocalDbContext _context;

        public BidRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<BidListDto>>> GetBidLists()
        {
            return await _context.BidLists
                .Select(bidList => new BidListDto
                {
                    BidListId = bidList.BidListId,
                    Account = bidList.Account,
                    BidType = bidList.BidType,
                    BidQuantity = bidList.BidQuantity
                })
                .ToListAsync();
        }

        public async Task<BidListDto?> GetBidList(int id)
        {
            var bidList = await _context.BidLists.FindAsync(id);

            if (bidList == null)
            {
                return null;
            }

            return new BidListDto
            {
                BidListId = bidList.BidListId,
                Account = bidList.Account,
                BidType = bidList.BidType,
                BidQuantity = bidList.BidQuantity
            };
        }
        
        public async Task<BidList?> UpdateBidList(int id, BidListDto bidListDto)
        {
            if (!BidListExists(id))
            {
                return null;
            }

            var bidList = await _context.BidLists.FindAsync(id);
            

            bidList!.Account = bidListDto.Account;
            bidList.BidType = bidListDto.BidType;
            bidList.BidQuantity = bidListDto.BidQuantity;

            _context.Set<BidList>().Update(bidList);
            await _context.SaveChangesAsync();
            return bidList;
        }
        
        public async Task<BidList> PostBidList(BidListDto bidListDto)
        {
            var bidList = new BidList
            {
                Account = bidListDto.Account,
                BidType = bidListDto.BidType,
                BidQuantity = bidListDto.BidQuantity,
            };

            _context.BidLists.Add(bidList);
            await _context.SaveChangesAsync();
            return bidList;
        }
        
        public async Task DeleteBidList(int id)
        {
            var bidList = await _context.BidLists.FindAsync(id);
            if (bidList != null) _context.BidLists.Remove(bidList);
            await _context.SaveChangesAsync();
        }
        
        public bool BidListExists(int id)
        {
            return _context.BidLists.Any(e => e.BidListId == id);
        }
    }

}