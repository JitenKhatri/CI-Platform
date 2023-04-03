using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using Microsoft.AspNetCore.Http;
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


        public (List<StoryViewModel>, int) GetAllStories(long user_id, int page = 1, int pageSize = 6)
        {
            int skipCount = (page - 1) * pageSize;

            var storiesQuery = _db.Stories.Where(s => s.Status == "PUBLISHED" || s.UserId == user_id);

            var storyQuery = storiesQuery
                .OrderBy(s => s.Status)
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
            int totalStories = storiesQuery.Count();

            return (stories, totalStories);
        }
        public List<StoryViewModel> GetFilteredStories(List<string> Countries, List<string> Cities, List<string> Themes, List<string> Skills, long user_id, string searchtext = null, int page = 1, int pageSize = 6)
        {
            int skipCount = (page - 1) * pageSize;

            var storiesQuery = _db.Stories
                .Where(s => s.Status == "PUBLISHED" || s.UserId == user_id);

            if (!string.IsNullOrEmpty(searchtext))
            {
                storiesQuery = storiesQuery.Where(s => s.Title.ToLower().Replace(" ", "").Contains(searchtext.ToLower().Replace(" ", ""))
                                                       || s.Description.ToLower().Replace(" ", "").Contains(searchtext.ToLower().Replace(" ", "")));
            }

            var cityQuery = _db.Cities.AsQueryable();

            if (Countries.Count > 0)
            {
                cityQuery = cityQuery.Where(c => Countries.Contains(c.Country.Name));
            }

            var storyQuery = storiesQuery
                .Where(s => Cities.Count == 0 || Cities.Contains(s.Mission.City.Name))
                .Where(s => Countries.Count == 0 || Countries.Contains(s.Mission.Country.Name))
                .Where(s => Themes.Count == 0 || Themes.Contains(s.Mission.Theme.Title))
                .Where(s => Skills.Count == 0 || _db.MissionSkills
                                                            .Where(ms => ms.MissionId == s.MissionId)
                                                            .Select(ms => ms.Skill.SkillName)
                                                            .All(sn => Skills.Contains(sn)));
                //.Where(s => Skills.Count == 0 || Skills.All(sm => s.Mission.MissionSkills.Any(k => k.Skill.SkillName == sm)));

            var stories = storyQuery
                .OrderBy(s => s.Status)
                .Skip(skipCount)
                .Take(pageSize)
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

                })
                .ToList();

            return stories;
        }

        public List<City> CityCascade(long countryid)
        {
            var cities = _db.Cities
                .Where(c => c.CountryId == countryid)
                .ToList();

            return cities;
        }
        public List<Mission> GetMissionApplications(long user_id)
        {

            List<Mission> missions = _db.MissionApplications
                                        .Where(ma => ma.UserId == user_id)
                                        .Select(ma => ma.Mission)
                                                           .ToList();
            return missions;
        }

        public bool ShareStory(long User_id,long storyId,long Mission_id,string title,string published_date,string story_description, List<IFormFile> storymedia,string type,List<String> videourl )
        {
            List<Story> stories = _db.Stories.ToList();
            List<StoryMedium> Storymedia = _db.StoryMedia.ToList();
            var existingstory = _db.Stories.FirstOrDefault(s => s.StoryId == storyId);
            if (existingstory == null)
            {
                var status = type == "PUBLISHED" ? "PUBLISHED" : "DRAFT";
                    Story story = new Story();
                    story.UserId = User_id;
                    story.Title = title;
                    story.Status = status;
                    story.Description = story_description;
                    story.PublishedAt = DateTime.Parse(published_date);
                    story.MissionId = Mission_id;
                    _db.Stories.Add(story);
                    _db.SaveChanges();


                    long story_id = story.StoryId;
                    foreach (var item in storymedia)
                    {
                        string uniqueFileName = null;
                        if (item != null)
                        {
                            // Get the uploaded file name
                            string fileName = Path.GetFileName(item.FileName);

                            // Create a unique file name to avoid overwriting existing files
                            uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

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
                foreach (var item in videourl)
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
                existingstory.UserId = User_id;
                existingstory.StoryId = storyId;
                existingstory.Title = title;
                existingstory.Description = story_description;
                existingstory.PublishedAt = DateTime.Parse(published_date);
                existingstory.Status = type == "PUBLISHED" ? "PUBLISHED" : "DRAFT";
                // Delete old media records related to the story
                var existingMedia = _db.StoryMedia.Where(sm => sm.StoryId == storyId);
                foreach (var medium in existingMedia)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", Path.GetFileName(medium.Path));
                    if (File.Exists(filePath))
                    {
                        string fileName = Path.GetFileName(medium.Path); // Extracts the file name from the file path
                        File.Delete(filePath);
                    }
                    _db.StoryMedia.Remove(medium);
                }
                long story_id = existingstory.StoryId;
                foreach (var item in storymedia)
                {
                    string uniqueFileName = null;
                    if (item != null)
                    {
                        // Get the uploaded file name
                        string fileName = Path.GetFileName(item.FileName);

                        // Create a unique file name to avoid overwriting existing files
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

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
                    foreach (var url in videourl)
                    {
                        if (url == null)
                        {
                            continue;
                        }

                        _db.StoryMedia.Add(new StoryMedium
                        {
                            StoryId = existingstory.StoryId,
                            Type = "vid",
                            Path = url
                        });
                    }
                }
                _db.SaveChanges();
                return true;

            }
        }

        public StoryViewModel GetStoryDetail(long user_id, long id)
        {
            var storyQuery = _db.Stories.Where(c => c.StoryId == id);
            var storyMediaQuery = _db.StoryMedia.Where(sm => sm.StoryId == id);
            var userQuery = _db.Users;

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
