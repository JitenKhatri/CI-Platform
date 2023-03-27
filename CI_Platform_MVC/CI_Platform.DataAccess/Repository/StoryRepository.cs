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
        public List<StoryViewModel> GetAllStories(long user_id,int page = 1, int pageSize = 6)
        {
            int skipCount = (page - 1) * pageSize;
            List<Story> stories = _db.Stories.Where(s => s.Status == "PUBLISHED" || s.UserId == user_id).Skip(skipCount).OrderBy(s=> s.Status)
                                          .Take(pageSize).ToList();
            List<StoryMedium> image = _db.StoryMedia.ToList();
            List<User> users = _db.Users.ToList();
            List<MissionTheme> themes = _db.MissionThemes.ToList();
            List<Mission> mission = _db.Missions.ToList();
            List<City> cities = _db.Cities.ToList();
            List<Country> countries = _db.Countries.ToList();
            var Stories = (from s in stories
                           join i in image on s.StoryId equals i.StoryId into data
                           from i in data.DefaultIfEmpty().Take(1)
                           select new StoryViewModel { image=i,Stories=s,Countries=countries,Cities=cities }).ToList();
            return Stories;
        }

        public List<StoryViewModel> GetFilteredStories(List<string> Countries, List<string> Cities, List<string> Themes, List<string> Skills, long user_id, string searchtext=null ,int page = 1, int pageSize = 6)
        {
            int skipCount = (page - 1) * pageSize;
            List<Story> stories = _db.Stories.Where(s => s.Status == "PUBLISHED" || s.UserId == user_id).Skip(skipCount).OrderBy(s => s.Status)
                                         .Take(pageSize).ToList();
            List<StoryMedium> image = _db.StoryMedia.ToList();
            List<MissionTheme> theme = _db.MissionThemes.ToList();
            List<Country> countries = _db.Countries.ToList();
            List<City> cities = _db.Cities.ToList();
            List<City> city = _db.Cities.ToList();

            List<User> users = _db.Users.ToList();
            List<Story> story = _db.Stories.ToList();
            List<MissionSkill> missionskills = _db.MissionSkills.ToList();
            List<Skill> skills = _db.Skills.ToList();
            List<Mission> mission = _db.Missions.ToList();

            if(!String.IsNullOrEmpty(searchtext))
            {
                stories = (from s in stories
                           select s).Where(s => s.Title.ToLower().Replace(" ", "").Contains(searchtext.ToLower().Replace(" ", ""))
                           || s.Description.ToLower().Replace(" ", "").Contains(searchtext.ToLower().Replace(" ", ""))).ToList();
            }
            if (Countries.Count > 0)
            {
                city = (from c in cities
                        where Countries.Contains(c.Country.Name)
                        select c).ToList();
            }
            else
            {
                city = cities;
            }
            story = (from s in stories
                     where ((Cities.Count == 0 || Cities.Contains(s.Mission.City.Name)) &&
                            (Countries.Count == 0 || Countries.Contains(s.Mission.Country.Name)) &&
                            (Themes.Count == 0 || Themes.Contains(s.Mission.Theme.Title)) && (Skills.Count == 0 || Skills.All(sm => s.Mission.MissionSkills.Any(k => k.Skill.SkillName == sm))))
                     select s).ToList();
           
            var Stories = (from s in story
                            join i in image on s.StoryId equals i.StoryId into data
                            from i in data.DefaultIfEmpty().Take(1)
                            select new StoryViewModel { image = i, Stories = s, Countries = countries, Cities = city, Story_city = s.Mission.City.Name, Story_theme = s.Mission.Theme.Title }).ToList();
            return Stories;

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

        public bool ShareStory(long User_id,long storyId,long Mission_id,string title,string published_date,string story_description, List<IFormFile> storymedia,string type )
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
                            Type = "imag",
                            Path = uniqueFileName // Save the unique file name in the database
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
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", medium.Path);
                    if (File.Exists(filePath))
                    {
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
                        Type = "imag",
                        Path = "/images/" + uniqueFileName // Save the unique file name in the database
                    });
                }
                _db.SaveChanges();
                return true;

            }
        }
           
           
        
    }
}
