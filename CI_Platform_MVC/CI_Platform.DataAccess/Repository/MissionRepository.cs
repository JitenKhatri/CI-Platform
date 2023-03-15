﻿using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository
{
    public class MissionRepository : Repository<Mission>, IMissionRepository
    {
        private readonly CiPlatformContext _db;
        public MissionRepository(CiPlatformContext db) : base(db)
        {
            _db = db;
        }

        
        public List<MissionViewModel> GetAllMission()
        {
            List<Mission> mission = _db.Missions.ToList();
           
            List<MissionMedium> image = _db.MissionMedia.ToList();
            List<MissionTheme> theme = _db.MissionThemes.ToList();
            List<Country> countries = _db.Countries.ToList();
            List<City> city = _db.Cities.ToList();
            List<Skill> skills = _db.Skills.ToList();
            var Missions = (from m in mission
                            join i in image on m.MissionId equals i.MissionId into data
                            from i in data.DefaultIfEmpty().Take(1)
                            select new MissionViewModel { image = i, Missions = m, Country = countries, themes = theme, skills = skills }).ToList();
            return Missions;
        }

        public List<MissionViewModel> GetFilteredMissions(List<string> Countries, List<string> Cities, List<string> Themes, List<string> Skills , string? sortOrder)
            
        {
            List<Mission> missions = _db.Missions.ToList();
            switch (sortOrder)
            {
                case null:
                case "Oldest":
                    sortOrder = "Oldest";
                    missions = _db.Missions.OrderBy(m => m.CreatedAt).ToList();
                    break;
                case "Newest":
                    missions = _db.Missions.OrderByDescending(m => m.CreatedAt).ToList();
                    break;
                case "Seats_ascending":
                    missions = _db.Missions.OrderBy(m => m.SeatsLeft).ToList();
                    break;
                case "Seats_descending":
                    missions = _db.Missions.OrderByDescending(m => m.SeatsLeft).ToList();
                    break;
                case "deadline":
                    missions = _db.Missions.OrderBy(m => m.Deadline).ToList();
                    break;
                default:
                    missions = _db.Missions.OrderBy(m => m.CreatedAt).ToList();
                    break;
            }
            List<MissionMedium> image = _db.MissionMedia.ToList();
            List<MissionTheme> theme = _db.MissionThemes.ToList();
            List<Country> countries = _db.Countries.ToList();
            List<City> cities = _db.Cities.ToList();
            List<City> city = new List<City>();
            List<Mission> mission = new List<Mission>();
            List<MissionSkill> missionskills = _db.MissionSkills.ToList();
            List<Skill> skills = _db.Skills.ToList();
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
            if (Cities.Count > 0)
            {
                mission = (from m in missions
                           where Cities.Contains(m.City.Name) || Themes.Contains(m.Theme.Title)
                           select m).ToList();
                var skill_missions = (from s in missionskills
                                      where Skills.Contains(s.Skill.SkillName)
                                      select s.Mission).ToList();
                foreach (var skill_mission in skill_missions)
                {
                    if (!mission.Contains(skill_mission))
                    {
                        mission.Add(skill_mission);
                    }
                }
            }
            else if (Countries.Count > 0 || Themes.Count > 0 || Skills.Count > 0)
            {
                mission = (from m in missions
                           where Countries.Contains(m.Country.Name) || Cities.Contains(m.City.Name) || Themes.Contains(m.Theme.Title)
                           select m).ToList();
                var skill_missions = (from s in missionskills
                                      where Skills.Contains(s.Skill.SkillName)
                                      select s.Mission).ToList();
                foreach (var skill_mission in skill_missions)
                {
                    if (!mission.Contains(skill_mission))
                    {
                        mission.Add(skill_mission);
                    }
                }
            }
            else
            {
                mission = missions;
            }
            var Missions = (from m in mission
                            join i in image on m.MissionId equals i.MissionId into data
                            from i in data.DefaultIfEmpty().Take(1)
                            select new MissionViewModel { image = i, Missions = m, Country = countries, Cities = city, Mission_city = m.City.Name, Mission_theme = m.Theme.Title }).ToList();
            return Missions;
        }

        public List<City> GetCitiesForCountry(long countryid)
        {
            var cities = _db.Cities
                .Where(c => c.CountryId == countryid)
                .ToList();

            return cities;
        }


        public List<MissionSkill> MissionSkillList(int id)
        {
            return _db.MissionSkills.Where(m => m.MissionId == id).ToList();
        }
        public IEnumerable<CommentViewModel> comment(long user_id, long mission_id, string comment)
        {

            List<Comment> comments = _db.Comments.ToList();
            List<User> users = _db.Users.ToList();
            Comment mycomment = new Comment()
            {
                UserId = user_id,
                MissionId = mission_id,
                CommentText = comment
            };
            _db.Comments.Add(mycomment);
            Save();
            comments = _db.Comments.ToList();
            IEnumerable<CommentViewModel> mission_comments = (from c in comments
                                                              where c.MissionId.Equals(mission_id)
                                                              select new CommentViewModel { User_Comment = c, user = c.User });
            return mission_comments;
        }

        public bool add_to_favourite(long user_id, long mission_id)
        {
            List<FavoriteMission> favoriteMissions = _db.FavoriteMissions.ToList(); 
            if (user_id != 0 && mission_id != 0)
            {
                var favouritemission = (from fm in favoriteMissions
                                        where fm.UserId.Equals(user_id) && fm.MissionId.Equals(mission_id)
                                        select fm).ToList();
                if (favouritemission.Count == 0)
                {
                    _db.FavoriteMissions.Add(new FavoriteMission
                    {
                        UserId = user_id,
                        MissionId = mission_id
                    });
                    Save();
                    return true;
                }
                else
                {
                    _db.Remove(favouritemission.ElementAt(0));
                    Save();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool Rate_mission(long user_id, long mission_id, int rating)
        {
            List<MissionRating> ratings = _db.MissionRatings.ToList();

            var Rating = (from r in ratings
                          where r.UserId.Equals(user_id) && r.MissionId.Equals(mission_id)
                          select r).ToList();
            if (Rating.Count == 0)
            {
                _db.MissionRatings.Add(new MissionRating
                {
                    UserId = user_id,
                    MissionId = mission_id,
                    Rating = rating.ToString()
                });
                Save();
                return true;
            }
            else
            {
                Rating.ElementAt(0).Rating = rating.ToString();
                Save();
                return true;
            }

        }
        public VolunteeringMissionVM GetMissionById(int id, long user_id)
        {
            List<MissionRating> ratings = _db.MissionRatings.ToList();
            List<FavoriteMission> favoriteMissions = _db.FavoriteMissions.ToList();
            int rating = 0;
            var Rating = (from r in ratings
                          where r.UserId.Equals(user_id) && r.MissionId.Equals(id)
                          select r).ToList();
            if (Rating.Count > 0)
            {
                rating = int.Parse(Rating.ElementAt(0).Rating);
            }
            var favouritemission = (from fm in favoriteMissions
                                    where fm.UserId.Equals(user_id) && fm.MissionId.Equals(id)
                                    select fm).ToList();
            List<User> users = _db.Users.ToList();
            Mission mission = _db.Missions.SingleOrDefault(m => m.MissionId == id);
            if (mission == null)
            {
                return null; // or throw an exception if desired
            }

            MissionMedium image = _db.MissionMedia.SingleOrDefault(i => i.MissionId == id);
            MissionTheme theme = _db.MissionThemes.SingleOrDefault(t => t.MissionThemeId == id);
            List<Skill> skills = _db.MissionSkills.Where(s => s.MissionSkillId == id).Select(s => s.Skill).ToList();
            List<Comment> comments = _db.Comments.Where(s => s.MissionId == id).ToList();
            Country country = _db.Countries.SingleOrDefault(c => c.CountryId == mission.CountryId);
            City city = _db.Cities.SingleOrDefault(c => c.CityId == mission.CityId);

            return new VolunteeringMissionVM
            {
                Missions = mission,
                image = image,
                theme = theme,
                skills = skills,
                Country = country,
                Cities = city,
                comments = comments,
                Rating = rating,
                Favorite_mission = favouritemission.Count
            };
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
