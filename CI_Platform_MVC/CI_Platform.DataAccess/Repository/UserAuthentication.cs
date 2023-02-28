using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository
{
    public class UserAuthentication : Repository<User>, IUserAuthentication
    {
        private readonly CiPlatformContext _db;
        public UserAuthentication(CiPlatformContext db) : base(db)
        {
            _db = db;
        }

        public User ResetPassword(string password, long id)
        {
            User user = _db.Users.Find(id);
            if (user == null)
            {
                return null;
            }
            else
            {
                user.Password = password;
                user.UpdatedAt = DateTime.Now;
                return user;
            }
        }
    }
}
