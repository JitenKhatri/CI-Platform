using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository
{
   public  class PasswordResetRepository : Repository<PasswordReset>, IPasswordResetRepository
    {
        private readonly CiPlatformContext _db;
        public PasswordResetRepository(CiPlatformContext db) : base(db)
        {
            _db = db;
        }

        public void Add(PasswordReset passwordReset)
        {
           _db.Set<PasswordReset>().Add(passwordReset);
            _db.SaveChanges();
        }

        public void Remove(PasswordReset passwordReset)
        {
            _db.PasswordResets.Remove(passwordReset);
            _db.SaveChanges();
        }
    }
}
