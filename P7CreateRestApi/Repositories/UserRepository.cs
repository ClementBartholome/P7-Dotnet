using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Data;

namespace P7CreateRestApi.Repositories
{
    public class UserRepository
    {
        private readonly LocalDbContext _context;

        public UserRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            return await _context.Users
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Password = user.Password,
                    FullName = user.FullName,
                    Role = user.Role
                })
                .ToListAsync();
        }

        public async Task<UserDto?> GetUser(int id)
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
                Password = user.Password, 
                FullName = user.FullName,
                Role = user.Role
            };
        }

        public async Task<User?> UpdateUser(int id, UserDto userDto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return null;
            }

            user.UserName = userDto.UserName;
            user.Password = userDto.Password; 
            user.FullName = userDto.FullName;
            user.Role = userDto.Role;

            _context.Set<User>().Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> PostUser(UserDto userDto)
        {
            var user = new User
            {
                UserName = userDto.UserName,
                Password = userDto.Password, 
                FullName = userDto.FullName,
                Role = userDto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUser(int id)
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
    }
}