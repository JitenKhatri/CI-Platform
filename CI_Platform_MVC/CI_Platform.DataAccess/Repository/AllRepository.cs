using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository
{
    public class AllRepository : IAllRepository
    {
        private readonly CiPlatformContext _db;
        public AllRepository(CiPlatformContext db)
        {
            _db = db;
            UserAuthentication = new UserAuthentication(_db);
            MissionRepository = new MissionRepository(_db);
            PasswordResetRepository = new PasswordResetRepository(_db);
        }
        public IUserAuthentication UserAuthentication { get; private set; }

        public IMissionRepository MissionRepository { get; private set; }

        public IPasswordResetRepository PasswordResetRepository { get; private set; }

        public void save()
        {
            _db.SaveChanges();
        }
    }
}
