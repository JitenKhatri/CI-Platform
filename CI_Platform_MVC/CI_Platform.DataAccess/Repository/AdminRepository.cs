using CI_Platform.Areas.Admin.ViewModels;
using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        private readonly CiPlatformContext _db;
        public AdminRepository(CiPlatformContext db) : base(db)
        {
            _db = db;
        }

        public UserCrudViewModel GetAllUsers()
        {
            var users = _db.Users.ToList();
            return new UserCrudViewModel
            {
                Users = users
            };
        }
    }
}
