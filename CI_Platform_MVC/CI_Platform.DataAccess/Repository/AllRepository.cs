using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AllRepository(CiPlatformContext db)
        {
            _db = db;
            UserAuthentication = new UserAuthentication(_db);
            MissionRepository = new MissionRepository(_db, _httpContextAccessor);
            PasswordResetRepository = new PasswordResetRepository(_db);
            StoryRepository = new StoryRepository(_db);
            AdminRepository = new AdminRepository(_db);
        }
        public IUserAuthentication UserAuthentication { get; private set; }

        public IMissionRepository MissionRepository { get; private set; }

        public IPasswordResetRepository PasswordResetRepository { get; private set; }
        public IStoryRepository StoryRepository { get; private set; }
        public IAdminRepository AdminRepository { get; private set; }

        public void save()
        {
            _db.SaveChanges();
        }
    }
}
