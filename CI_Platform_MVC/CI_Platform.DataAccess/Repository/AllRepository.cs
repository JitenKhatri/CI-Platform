using CI_Platform.DataAccess.Repository.IRepository;
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
            Mission = new Mission(_db);
        }
        public IUserAuthentication UserAuthentication { get; private set; }

        public IMission Mission { get; private set; }

        public void save()
        {
            _db.SaveChanges();
        }
    }
}
