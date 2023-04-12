using CI_Platform.Areas.Admin.ViewModels;
using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public CrudViewModel GetAllUsers()
        {
            var users = _db.Users.ToList();
            return new CrudViewModel
            {
                Users = users
            };
        }
        public CrudViewModel GetAllMissions()
        {
            var missions = _db.Missions.ToList();
            return new CrudViewModel
            {
                Missions = missions
            };
        }

        public CrudViewModel GetAllThemes()
        {
            var missionThemes = _db.MissionThemes.ToList();
            return new CrudViewModel
            {
                MissionThemes = missionThemes
            };
        }

        public CrudViewModel GetAllSkills()
        {
            List<Skill> Skills = _db.Skills.ToList();
            return new CrudViewModel
            {
                Skills = Skills
            };
        }

        public CrudViewModel GetAllStories()
        {
            var stories = _db.Stories
                            .Select(s => new Story
                            {
                                StoryId = s.StoryId,
                                Title = s.Title,
                                // Include related Mission entity
                                Mission = _db.Missions.FirstOrDefault(m => m.MissionId == s.MissionId),
                                // Include related User entity
                                User = _db.Users.FirstOrDefault(u => u.UserId == s.UserId)
                            })
                            .ToList();

            return new CrudViewModel
            {
                Stories = stories
            };
        }
    }
}
