using CI_Platform.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> UserExistsAsync(string email, string password);
        Task AddUserAsync(User user);
        Task SaveChangesAsync();
    }
}
