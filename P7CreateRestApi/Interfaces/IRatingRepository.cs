using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Models.Dto;

namespace P7CreateRestApi.Repositories;

public interface IRatingRepository
{
    Task<ActionResult<IEnumerable<RatingDto>>> GetRatings();
    Task<RatingDto?> GetRating(int id);
    Task<Rating?> UpdateRating(int id, RatingDto ratingDto);
    Task<Rating> PostRating(RatingDto ratingDto);
    Task<bool> DeleteRating(int id);
    bool RatingExists(int id);
}