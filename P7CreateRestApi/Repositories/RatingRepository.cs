using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Data;
using P7CreateRestApi.Models.Dto;

namespace P7CreateRestApi.Repositories
{
    public class RatingRepository
    {
        private readonly LocalDbContext _context;

        public RatingRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<RatingDto>>> GetRatings()
        {
            return await _context.Ratings
                .Select(rating => new RatingDto
                {
                    Id = rating.Id,
                    MoodysRating = rating.MoodysRating,
                    SandPRating = rating.SandPRating,
                    FitchRating = rating.FitchRating,
                    OrderNumber = rating.OrderNumber
                })
                .ToListAsync();
        }

        public async Task<RatingDto?> GetRating(int id)
        {
            var rating = await _context.Ratings.FindAsync(id);

            if (rating == null)
            {
                return null;
            }

            return new RatingDto
            {
                Id = rating.Id,
                MoodysRating = rating.MoodysRating,
                SandPRating = rating.SandPRating,
                FitchRating = rating.FitchRating,
                OrderNumber = rating.OrderNumber
            };
        }

        public async Task<Rating?> UpdateRating(int id, RatingDto ratingDto)
        {
            if (!RatingExists(id))
            {
                return null;
            }

            var rating = await _context.Ratings.FindAsync(id);

            if (rating == null) return null;
            
            rating.MoodysRating = ratingDto.MoodysRating;
            rating.SandPRating = ratingDto.SandPRating;
            rating.FitchRating = ratingDto.FitchRating;
            rating.OrderNumber = ratingDto.OrderNumber;

            _context.Set<Rating>().Update(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<Rating> PostRating(RatingDto ratingDto)
        {
            var rating = new Rating
            {
                MoodysRating = ratingDto.MoodysRating,
                SandPRating = ratingDto.SandPRating,
                FitchRating = ratingDto.FitchRating,
                OrderNumber = ratingDto.OrderNumber
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<bool> DeleteRating(int id)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null)
            {
                return false;
            }

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();
            return true;
        }

        private bool RatingExists(int id)
        {
            return _context.Ratings.Any(e => e.Id == id);
        }
    }
}