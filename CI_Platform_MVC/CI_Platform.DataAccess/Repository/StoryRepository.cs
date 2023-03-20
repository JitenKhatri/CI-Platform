using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository
{
    public class StoryRepository : Repository<Story>, IStoryRepository
    {
        private readonly CiPlatformContext _db;
        public StoryRepository(CiPlatformContext db) : base(db)
        {
            _db = db;
        }
        public List<StoryViewModel> GetAllStories(int page = 1, int pageSize = 6)
        {
            int skipCount = (page - 1) * pageSize;
            List<Story> stories = _db.Stories.Skip(skipCount)
                                          .Take(pageSize).ToList();
            List<StoryMedium> image = _db.StoryMedia.ToList();
            List<User> users = _db.Users.ToList();
            List<MissionTheme> themes = _db.MissionThemes.ToList();
            List<Mission> mission = _db.Missions.ToList();
            var Stories = (from s in stories
                           join i in image on s.StoryId equals i.StoryId into data
                           from i in data.DefaultIfEmpty().Take(1)
                           select new StoryViewModel { image=i,Stories=s }).ToList();
            return Stories;
        }
    }
}
