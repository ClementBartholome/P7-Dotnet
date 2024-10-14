using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Data;
using P7CreateRestApi.Models.Dto;

namespace P7CreateRestApi.Repositories;

public class RuleRepository : IRuleRepository
{
    private readonly LocalDbContext _context;

    public RuleRepository(LocalDbContext context)
    {
        _context = context;
    }

    public async Task<ActionResult<IEnumerable<RuleDto>>> GetRules()
    {
        return await _context.RuleNames
            .Select(rule => new RuleDto
            {
                Id = rule.Id,
                Name = rule.Name,
                Description = rule.Description,
                Json = rule.Json,
                Template = rule.Template,
                SqlStr = rule.SqlStr,
                SqlPart = rule.SqlPart
            })
            .ToListAsync();
    }

    public async Task<RuleDto?> GetRule(int id)
    {
        var rule = await _context.RuleNames.FindAsync(id);

        if (rule == null)
        {
            return null;
        }

        return new RuleDto
        {
            Id = rule.Id,
            Name = rule.Name,
            Description = rule.Description,
            Json = rule.Json,
            Template = rule.Template,
            SqlStr = rule.SqlStr,
            SqlPart = rule.SqlPart
        };
    }
    
    public async Task<RuleName?> UpdateRule(int id, RuleDto ruleDto)
    {
        if (!RuleExists(id))
        {
            return null;
        }

        var rule = await _context.RuleNames.FindAsync(id);

        if (rule != null)
        {
            rule.Name = ruleDto.Name;
            rule.Description = ruleDto.Description;
            rule.Json = ruleDto.Json;
            rule.Template = ruleDto.Template;
            rule.SqlStr = ruleDto.SqlStr;
            rule.SqlPart = ruleDto.SqlPart;

            _context.Set<RuleName>().Update(rule);
            await _context.SaveChangesAsync();
        }

        return rule;
    }
    
    public async Task<RuleName> PostRule(RuleDto ruleDto)
    {
        var rule = new RuleName
        {
            Name = ruleDto.Name,
            Description = ruleDto.Description,
            Json = ruleDto.Json,
            Template = ruleDto.Template,
            SqlStr = ruleDto.SqlStr,
            SqlPart = ruleDto.SqlPart
        };

        _context.RuleNames.Add(rule);
        await _context.SaveChangesAsync();
        return rule;
    }
    
    public async Task<bool> DeleteRule(int id)
    {
        var rule = await _context.RuleNames.FindAsync(id);

        if (rule == null)
        {
            return false;
        }


        _context.RuleNames.Remove(rule);
        await _context.SaveChangesAsync();
        return true;
    }

    public bool RuleExists(int id)
    {
        return _context.RuleNames.Any(e => e.Id == id);
    }   
}