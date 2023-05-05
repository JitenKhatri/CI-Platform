using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.InputModels;
using CI_Platform.Models.ViewModels;

namespace CI_Platform.DataAccess.Repository
{
    public class StoryRepository : Repository<Story>, IStoryRepository
    {
        private readonly CiPlatformContext _db;
        public StoryRepository(CiPlatformContext db) : base(db)
        {
            _db = db;
        }


        public (List<StoryViewModel>, int) GetAllStories(long user_id, int page = 1, int pageSize = 3)
        {
            int skipCount = (page - 1) * pageSize;

            var storiesQuery = _db.Stories.Where(story => story.DeletedAt == null && (story.UserId == user_id || story.Status == "PUBLISHED"));
            int totalStories = storiesQuery.Count();
            var storyQuery = storiesQuery
                .OrderByDescending(s => s.Status == "DRAFT")
                .Skip(skipCount)
                .Take(pageSize)
                .Select(s => new StoryViewModel
                {
                    Stories = s,
                    Image = s.StoryMedia.FirstOrDefault(),
                    Countries = _db.Countries.ToList(),
                    Cities = _db.Cities.ToList(),
                    Story_city = s.Mission.City.Name,
                    Story_theme = s.Mission.Theme.Title,
                    User_firstname = s.User.FirstName, // include fields from User table
                    User_lastname = s.User.LastName,
                    User_avatar = s.User.Avatar,
                    User_id = s.User.UserId,
                    Images = s.StoryMedia.Where(sm => sm.Type == "img").ToList(),
                    MissionId = s.Mission.MissionId,
                    Missiontitle = s.Mission.Title,
                    Vidmedia = s.StoryMedia.Where(sm => sm.Type == "vid").ToList()

                }) ; ;

            var stories = storyQuery.ToList();
            

            return (stories, totalStories);
        }
        public (List<StoryViewModel>,int) GetFilteredStories(StoryInputModel model)
        {
            int skipCount = (model.Page - 1) * model.PageSize;

            var storiesQuery = _db.Stories
                .Where(story => story.DeletedAt == null && (story.UserId == model.UserId || story.Status == "PUBLISHED"));

            if (!string.IsNullOrEmpty(model.SearchText))
            {
                storiesQuery = storiesQuery.Where(s => s.Title.ToLower().Replace(" ", "").Contains(model.SearchText.ToLower().Replace(" ", ""))
                                                       || s.Description.ToLower().Replace(" ", "").Contains(model.SearchText.ToLower().Replace(" ", "")));
            }

            var cityQuery = _db.Cities.AsQueryable();

            if (model.Countries.Count > 0)
            {
                cityQuery = cityQuery.Where(c => model.Countries.Contains(c.Country.Name));
            }


            var storyQuery = storiesQuery
                .Where(s => model.Cities.Count == 0 || model.Cities.Contains(s.Mission.City.Name))
                .Where(s => model.Countries.Count == 0 || model.Countries.Contains(s.Mission.Country.Name))
                .Where(s => model.Themes.Count == 0 || model.Themes.Contains(s.Mission.Theme.Title));

            if (model.Skills.Count > 0)
            {
                var missionIds = _db.MissionSkills
                    .Where(ms => model.Skills.Contains(ms.Skill.SkillName))
                    .Select(ms => ms.MissionId)
                    .Distinct();
                storyQuery = storyQuery.Where(s => missionIds.Contains(s.MissionId));
            }

            int totalcount = storyQuery.Count();
            var stories = storyQuery
                .OrderBy(s => s.Status)
                .Skip(skipCount)
                .Take(model.PageSize)
                .Select(s => new StoryViewModel
                {
                    Stories = s,
                    Image = s.StoryMedia.FirstOrDefault(),
                    Countries = _db.Countries.ToList(),
                    Cities = cityQuery.ToList(),
                    Story_city = s.Mission.City.Name,
                    Story_theme = s.Mission.Theme.Title,
                    User_avatar = s.User.Avatar,
                    User_id = s.User.UserId,
                    User_firstname = s.User.FirstName,
                    User_lastname = s.User.LastName,
                    Images = s.StoryMedia.Where(sm => sm.Type == "img").ToList(),
                    MissionId = s.Mission.MissionId,
                    Missiontitle = s.Mission.Title,
                    Vidmedia = s.StoryMedia.Where(sm => sm.Type == "vid").ToList()

                })
                .ToList();

            return (stories, totalcount);
        }

        public List<City> CityCascade(long countryid)
        {
            if (countryid != 0)
            {
                var cities = _db.Cities
                 .Where(c => c.CountryId == countryid)
                 .ToList();

                return cities;
            }
            else
            {
                var Cities = _db.Cities.ToList();
                return Cities;
            }
        }
        public List<Mission> GetMissionApplications(long user_id)
        {

            List<Mission> missions = _db.MissionApplications
                                        .Where(ma => ma.UserId == user_id)
                                        .Select(ma => ma.Mission)
                                                           .ToList();
            return missions;
        }

        public bool ShareStory(StoryInputModel model)
        {
            List<Story> stories = _db.Stories.ToList();
            List<StoryMedium> Storymedia = _db.StoryMedia.ToList();
            var existingstory = _db.Stories.FirstOrDefault(s => s.StoryId == model.StoryId);
            if (existingstory == null)
            {
                var status = model.Type == "PUBLISHED" ? "PUBLISHED" : "DRAFT";
                Story story = new Story();
                story.UserId = model.UserId;
                story.Title = model.Title;
                story.Status = status;
                story.Description = model.StoryDescription;
                story.PublishedAt = DateTime.Parse(model.PublishedDate);
                story.MissionId = model.MissionId;
                _db.Stories.Add(story);
                _db.SaveChanges();


                long story_id = story.StoryId;
                foreach (var item in model.Media)
                {
                    string uniqueFileName = null;
                    if (item != null)
                    {
                        // Get the uploaded file name
                        string fileName = Path.GetFileName(item.FileName);
                        Random random = new Random();
                        string randomString = new string(
                            Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 4)
                                      .Select(s => s[random.Next(s.Length)])
                                      .ToArray()
                        );

                        // Create a unique file name to avoid overwriting existing files
                        uniqueFileName = randomString + "_" + fileName;

                        // Set the file path where the uploaded file will be saved
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFileName);

                        // Save the uploaded file to the specified directory
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            item.CopyTo(fileStream);
                        }
                    }

                    _db.StoryMedia.Add(new StoryMedium
                    {
                        StoryId = story_id,
                        Type = "img",
                        Path = "/images/" + uniqueFileName // Save the unique file name in the database
                    });
                }
                foreach (var item in model.VideoUrls)
                {

                    _db.StoryMedia.Add(new StoryMedium
                    {
                        StoryId = story.StoryId,
                        Type = "vid",
                        Path = item
                    });
                }
                _db.SaveChanges();
                return true;
            }


            else
            {
                existingstory.UserId = model.UserId;
                existingstory.StoryId = model.StoryId;
                existingstory.Title = model.Title;
                existingstory.Description = model.StoryDescription;
                existingstory.PublishedAt = DateTime.Parse(model.PublishedDate);
                existingstory.Status = model.Type == "PUBLISHED" ? "PUBLISHED" : "DRAFT";
                // Delete old media records related to the story
                var existingMedia = _db.StoryMedia.Where(sm => sm.StoryId == model.StoryId);
                foreach (var medium in existingMedia)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", medium.Path);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    _db.StoryMedia.Remove(medium);
                }
                long story_id = existingstory.StoryId;
                foreach (var item in model.Media)
                {
                    string uniqueFileName = null;
                    if (item != null)
                    {
                        // Get the uploaded file name
                        string fileName = Path.GetFileName(item.FileName);
                        Random random = new Random();
                        string randomString = new string(
                            Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 4)
                                      .Select(s => s[random.Next(s.Length)])
                                      .ToArray()
                        );

                        // Create a unique file name to avoid overwriting existing files
                        uniqueFileName = randomString + "_" + fileName;

                        // Set the file path where the uploaded file will be saved
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFileName);

                        // Save the uploaded file to the specified directory
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            item.CopyTo(fileStream);
                        }
                    }

                    _db.StoryMedia.Add(new StoryMedium
                    {
                        StoryId = story_id,
                        Type = "img",
                        Path = "/images/" + uniqueFileName // Save the unique file name in the database
                    });
                    
                }
                foreach (var url in model.VideoUrls)
                {
                    _db.StoryMedia.Add(new StoryMedium
                    {
                        StoryId = existingstory.StoryId,
                        Type = "vid",
                        Path = url
                    });
                }
                _db.SaveChanges();
                return true;

            }
        }

        public StoryViewModel GetStoryDetail(long user_id, long id)
        {
            var storyQuery = _db.Stories.Where(c => c.StoryId == id);
            var storyMediaQuery = _db.StoryMedia.Where(sm => sm.StoryId == id);
            var userQuery = _db.Users.Where(User => User.DeletedAt == null);

            var story = storyQuery.FirstOrDefault();
            var storyMedia = storyMediaQuery.ToList();
            var users = userQuery.ToList();

            return new StoryViewModel { Stories = story, All_volunteers = users };
        }
   

        public void Add_View(long user_id, long story_id)
        {
            List<StoryView> storyviews = _db.StoryViews.ToList();
            var view_exist = _db.StoryViews.FirstOrDefault(c => c.UserId.Equals(user_id) && c.StoryId.Equals(story_id));
            if (view_exist is null)
            {
                _db.StoryViews.Add(new StoryView { StoryId = story_id, UserId = user_id });
                _db.SaveChanges();
            }
        }

        public bool Recommend(long user_id, long story_id,long to_user_id)
        {
                _db.StoryInvites.Add(new StoryInvite
                {
                    FromUserId = user_id,
                    ToUserId = to_user_id,
                    StoryId = story_id
                });

            _db.SaveChanges();
            return true;
        }

    }
}
