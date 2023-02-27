using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly CiPlatformContext _context;
        private readonly UserManager<User> _userManager;
        

        public UserRepository(CiPlatformContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> UserExistsAsync(string email, string password)
        {
            return await _dbSet.AnyAsync(u => u.Email == email && u.Password == password);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

        public void UpdateUser(User user)
        {
            _context.Set<User>().Update(user);
            _context.SaveChanges();
        }
        //public async Task GenerateForgotPasswordTokenAsync(User user)
        //{
        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        await SendForgotPasswordEmail(user, token);
        //    }
        //}

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public Task GenerateForgotPasswordTokenAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}




