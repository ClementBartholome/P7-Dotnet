using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Models.Dto;


namespace P7CreateRestApi.Repositories
{
    public class CurveRepository
    {
        private readonly LocalDbContext _context;

        public CurveRepository(LocalDbContext context)
        {
            _context = context;
        }
        
        public async Task<ActionResult<IEnumerable<CurvePointDto>>> GetCurves()
        {
            return await _context.CurvePoints
                .Select(curvePoint => new CurvePointDto
                {
                    Id = curvePoint.Id,
                    CurveId = curvePoint.CurveId,
                    Term = curvePoint.Term,
                    CurvePointValue = curvePoint.CurvePointValue
                })
                .ToListAsync();
        }
        
        public async Task<CurvePointDto?> GetCurve(int id)
        {
            var curvePoint = await _context.CurvePoints.FindAsync(id);

            if (curvePoint == null)
            {
                return null;
            }

            return new CurvePointDto
            {
                Id = curvePoint.Id,
                CurveId = curvePoint.CurveId,
                Term = curvePoint.Term,
                CurvePointValue = curvePoint.CurvePointValue
            };
        }
        
        public async Task<CurvePoint?> UpdateCurve(int id, CurvePointDto curvePointDto)
        {
            if (!CurveExists(id))
            {
                return null;
            }

            var curvePoint = await _context.CurvePoints.FindAsync(id);

            if (curvePoint == null) return curvePoint;
            curvePoint.CurveId = curvePointDto.CurveId;
            curvePoint.Term = curvePointDto.Term;
            curvePoint.CurvePointValue = curvePointDto.CurvePointValue;
            await _context.SaveChangesAsync();

            return curvePoint;
        }
        
        public async Task<CurvePoint> PostCurve(CurvePointDto curvePointDto)
        {
            var curvePoint = new CurvePoint
            {
                CurveId = curvePointDto.CurveId,
                Term = curvePointDto.Term,
                CurvePointValue = curvePointDto.CurvePointValue
            };

            _context.CurvePoints.Add(curvePoint);
            await _context.SaveChangesAsync();
            return curvePoint;
        }

        public async Task<bool> DeleteCurve(int id)
        {
            var curvePoint = await _context.CurvePoints.FindAsync(id);
            if (curvePoint == null)
            {
                return false;
            }

            _context.CurvePoints.Remove(curvePoint);
            await _context.SaveChangesAsync();
            return true;
        }
        
        private bool CurveExists(int id)
        {
            return _context.CurvePoints.Any(e => e.Id == id);
        }
        
    }
}