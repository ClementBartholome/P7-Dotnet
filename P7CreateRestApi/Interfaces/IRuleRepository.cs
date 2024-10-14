using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Models.Dto;

namespace P7CreateRestApi.Repositories;

public interface IRuleRepository
{
    Task<ActionResult<IEnumerable<RuleDto>>> GetRules();
    Task<RuleDto?> GetRule(int id);
    Task<RuleName?> UpdateRule(int id, RuleDto ruleDto);
    Task<RuleName> PostRule(RuleDto ruleDto);
    Task<bool> DeleteRule(int id);
    bool RuleExists(int id);
}