using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
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
    }
}




