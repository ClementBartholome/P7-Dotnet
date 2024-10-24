using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using Microsoft.AspNetCore.Identity;
using P7CreateRestApi.Interfaces;

namespace P7CreateRestApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LocalDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(LocalDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUser(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> UpdateUser(string id, UserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return null;
            }

            user.UserName = userDto.UserName;
            user.FullName = userDto.FullName;

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userDto.Password);
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return null;
            }

            return user;
        }

        public async Task<bool> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            var result = _userManager.DeleteAsync(user);
            return result.Result.Succeeded;
        }

        public async Task<(bool Success, List<string> AlreadyInRoles)> AddRolesToUser(string id, List<string> roles)
        {
            if (roles == null || roles.Count == 0)
            {
                throw new ArgumentException("Roles list cannot be null or empty", nameof(roles));
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return (false, new List<string>());
            }

            var alreadyInRoles = new List<string>();
            foreach (var role in roles)
            {
                if (await _userManager.IsInRoleAsync(user, role))
                {
                    alreadyInRoles.Add(role);
                }
                else
                {
                    var result = await _userManager.AddToRoleAsync(user, role);
                    if (!result.Succeeded)
                    {
                        return (false, alreadyInRoles);
                    }
                }
            }

            return (true, alreadyInRoles);
        }
        
        public async Task<bool> RemoveRoleFromUser(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            if (!await _userManager.IsInRoleAsync(user, role))
            {
                return true;
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            return result.Succeeded;
        }


        public async Task<bool> UpdateRoleForUser(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count == 0)
            {
                return false;
            }

            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                return false;
            }

            result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }
        
        public async Task<bool> CreateRole(string roleName)
        {
            var roleExists = await _context.Roles.AnyAsync(r => r.Name == roleName);
            if (roleExists)
            {
                return false;
            }

            var role = new IdentityRole(roleName);
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRole(string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
            {
                return false;
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}