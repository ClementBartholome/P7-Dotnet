using P7CreateRestApi.Models.Dto;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;

namespace P7CreateRestApi.Interfaces
{
    public interface ICurveRepository
    {
        Task<ActionResult<IEnumerable<CurvePointDto>>> GetCurves();
        Task<CurvePointDto?> GetCurve(int id);
        Task<CurvePoint?> UpdateCurve(int id, CurvePointDto curvePointDto);
        Task<CurvePoint> PostCurve(CurvePointDto curvePointDto);
        Task<bool> DeleteCurve(int id);
        bool CurveExists(int id);
    }
}