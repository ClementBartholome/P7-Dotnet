using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using Microsoft.AspNetCore.Identity;

namespace P7CreateRestApi.Repositories
{
    public class UserRepository
    {
        private readonly LocalDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(LocalDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            return await _context.Users
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Roles = _userManager.GetRolesAsync(user).Result,
                })
                .ToListAsync();
        }

        public async Task<UserDto?> GetUser(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Roles = _userManager.GetRolesAsync(user).Result,
            };
        }

        public async Task<User?> UpdateUser(string id, UserDto userDto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return null;
            }

            user.UserName = userDto.UserName;
            user.PasswordHash = userDto.Password;
            user.FullName = userDto.FullName;

            _context.Set<User>().Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> PostUser(UserDto userDto)
        {
            var user = new User
            {
                UserName = userDto.UserName,
                PasswordHash = userDto.Password,
                FullName = userDto.FullName,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(bool Success, List<string> AlreadyInRoles)> AddRolesToUser(string id, List<string> roles)
        {
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