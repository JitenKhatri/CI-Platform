﻿using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
  
        public List<MissionViewModel> GetAllMission(int page = 1, int pageSize = 6)
        {
            int skipCount = (page - 1) * pageSize;
            List<Mission> mission = _db.Missions.Skip(skipCount)
                                          .Take(pageSize).ToList();
           
            List<MissionMedium> image = _db.MissionMedia.ToList();
            List<MissionTheme> theme = _db.MissionThemes.ToList();
            List<Country> countries = _db.Countries.ToList();
            List<City> city = _db.Cities.ToList();
            List<Skill> skills = _db.Skills.ToList();
            List<FavoriteMission> favoriteMissions = _db.FavoriteMissions.ToList();
            List<MissionRating> missionRatings = _db.MissionRatings.ToList();
            var Missions = (from m in mission
                            join i in image on m.MissionId equals i.MissionId into data
                            from i in data.DefaultIfEmpty().Take(1)
                            select new MissionViewModel { image = i, Missions = m, Country = countries, themes = theme, skills = skills,favoriteMissions=favoriteMissions,missionRatings=missionRatings }).ToList();
            return Missions;
        }

        public List<MissionViewModel> GetFilteredMissions(List<string> Countries, List<string> Cities, List<string> Themes, List<string> Skills , string? sortOrder, string searchtext = null, int page=1 , int pageSize = 6)
            {
            int skipCount = (page - 1) * pageSize;
            List<Mission> missions = _db.Missions.ToList();
            switch (sortOrder)
            {
                case null:
                case "Oldest":
                    sortOrder = "Oldest";
                    missions = _db.Missions.OrderBy(m => m.CreatedAt).Skip(skipCount)
                                          .Take(pageSize).ToList();
                    break;
                case "Newest":
                    missions = _db.Missions.OrderByDescending(m => m.CreatedAt).Skip(skipCount)
                                          .Take(pageSize).ToList();
                    break;
                case "Seats_ascending":
                    missions = _db.Missions.OrderBy(m => m.SeatsLeft).Skip(skipCount)
                                          .Take(pageSize).ToList();
                    break;
                case "Seats_descending":
                    missions = _db.Missions.OrderByDescending(m => m.SeatsLeft).Skip(skipCount)
                                          .Take(pageSize).ToList();
                    break;
                case "deadline":
                    missions = _db.Missions.OrderBy(m => m.Deadline).Skip(skipCount)
                                          .Take(pageSize).ToList();
                    break;
                default:
                    missions = _db.Missions.OrderBy(m => m.CreatedAt).Skip(skipCount)
                                          .Take(pageSize).ToList();
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
            mission = (from m in missions
                     where ((Cities.Count == 0 || Cities.Contains(m.City.Name)) &&
                            (Countries.Count == 0 || Countries.Contains(m.Country.Name)) &&
                            (Themes.Count == 0 || Themes.Contains(m.Theme.Title)) && (Skills.Count == 0 || Skills.All(sm => m.MissionSkills.Any(k => k.Skill.SkillName == sm))))
                     select m).ToList();
            if (!String.IsNullOrEmpty(searchtext))
            {
                mission = (from m in missions
                           select m).Where(m => m.Title.ToLower().Replace(" ", "").Contains(searchtext.ToLower().Replace(" ", ""))
                           || m.Description.ToLower().Replace(" ", "").Contains(searchtext.ToLower().Replace(" ", ""))).ToList();
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
        public bool apply_for_mission(long user_id, long mission_id)
        {
            List<MissionApplication> missionApplications = _db.MissionApplications.ToList();
            DateTime current = DateTime.Now;
            if (user_id != 0 && mission_id != 0)
            {
                var missionapplication = (from ma in missionApplications
                                          where ma.UserId.Equals(user_id) && ma.MissionId.Equals(mission_id)
                                          select ma).ToList();
                if (missionapplication.Count == 0)
                {
                    _db.MissionApplications.Add(new MissionApplication
                    {
                        AppliedAt = current,
                        UserId = user_id,
                        MissionId = mission_id
                    });
                    Save();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public VolunteeringMissionVM Next_Volunteers(int count, long user_id, long mission_id)
        {
            List<MissionApplication> missionApplications = _db.MissionApplications.ToList();
            List<User> Users = _db.Users.ToList();
            Mission? mission = _db.Missions.Find(mission_id);
            List<User> users = (from ma in missionApplications
                                where ma.MissionId.Equals(mission?.MissionId) && !ma.UserId.Equals(user_id)
                                select ma.User).ToList();
            return new VolunteeringMissionVM { Missions = mission, Recent_volunteers = users.Skip(1 * count).Take(1).ToList(), Total_volunteers = users.Count };
        }
        public bool Recommend(long user_id, long mission_id, List<long> co_workers)
        {
            foreach (var user in co_workers)
            {
                _db.MissionInvites.Add(new MissionInvite
                {
                    FromUserId = user_id,
                    ToUserId = user,
                    MissionId = mission_id
                });
            }
            Save();
            return true;
        }
        public VolunteeringMissionVM GetMissionById(int id, long user_id)
        {
            List <MissionRating> ratings = _db.MissionRatings.ToList();
            List<FavoriteMission> favoriteMissions = _db.FavoriteMissions.ToList();
            List<Mission> missions = _db.Missions.ToList();
            List<MissionTheme> themes = _db.MissionThemes.ToList();
            List<Country> countries = _db.Countries.ToList();
            List<City> cities = _db.Cities.ToList();
            List<MissionMedium> images = _db.MissionMedia.ToList();
            List<MissionDocument> documents = _db.MissionDocuments.ToList();
            List<User> users = _db.Users.ToList();
            List<Mission> related_mission = _db.Missions.ToList();
            List<MissionApplication> missionApplications = _db.MissionApplications.ToList();
            List<User> all_volunteers = _db.Users.ToList();
            List<User> already_recommended = new List<User>();
            List<MissionInvite> already_recommended_users = new List<MissionInvite>();

            decimal avg_ratings = 0;
            int rating_count = 0;
            int rating = 0;
            var Rating = (from r in ratings
                          where r.UserId.Equals(user_id) && r.MissionId.Equals(id)
                          select r).ToList();
            bool applied_or_not = false;
            if (Rating.Count > 0)
            {
                rating = int.Parse(Rating.ElementAt(0).Rating);
            }
           


            Mission mission = _db.Missions.SingleOrDefault(m => m.MissionId == id);
            if (mission == null)
            {
                return null; // or throw an exception if desired
            }

            MissionMedium image = _db.MissionMedia.SingleOrDefault(i => i.MissionId == id);
            MissionTheme theme = _db.MissionThemes.SingleOrDefault(t => t.MissionThemeId == mission.ThemeId);
            List<Skill> skills = _db.MissionSkills.Where(s => s.MissionId == id).Select(s => s.Skill).ToList();
            List<Comment> comments = _db.Comments.Where(s => s.MissionId == id).Take(5).ToList();
            List<MissionDocument> missiondocuments = _db.MissionDocuments.Where(ms => ms.MissionId == id).ToList();
            Country country = _db.Countries.SingleOrDefault(c => c.CountryId == mission.CountryId);
            City city = _db.Cities.SingleOrDefault(c => c.CityId == mission.CityId);

            if (mission.MissionRatings.Count > 0)
            {
                avg_ratings = (from m in mission.MissionRatings
                               select decimal.Parse(m.Rating)).Average();
                rating_count = (from m in mission.MissionRatings
                                select m).ToList().Count;
            }
            List<User> volunteers = (from ma in missionApplications
                                     where ma.MissionId.Equals(mission?.MissionId) && !ma.UserId.Equals(user_id)
                                     select ma.User).ToList();



            related_mission = (from m in missions
                                   where !m.MissionId.Equals(mission.MissionId) && m.City?.Name != null && m.City.Name.Equals(mission.City.Name)
                                   select m).Take(3).ToList();
                if (related_mission.Count == 0)
                {
                    related_mission = (from m in missions
                                       where !m.MissionId.Equals(mission.MissionId) && m.Country?.Name !=null && m.Country.Name.Equals(mission.Country.Name) 
                                       select m).Take(3).ToList();
                    if (related_mission.Count == 0)
                    {
                        related_mission = (from m in missions
                                           where !m.MissionId.Equals(mission.MissionId) && m.Theme?.Title !=null && m.Theme.Title.Equals(mission.Theme.Title)
                                           select m).Take(3).ToList();
                            
                    }
                    else
                    {
                        related_mission = null;
                    }
                }
            List<MissionApplication> applied = (from ma in missionApplications
                                                where ma.MissionId.Equals(mission?.MissionId) && ma.UserId.Equals(user_id)
                                                select ma).ToList();
            if (applied.Count > 0)
            {
                applied_or_not = true;
            }
            var favouritemission = (from fm in favoriteMissions
                                    where fm.UserId.Equals(user_id) && fm.MissionId.Equals(id)
                                    select fm).ToList();

            //if (already_recommended_users.Count > 0)
            //{
            //    foreach (var item in already_recommended_users)
            //    {
            //        already_recommended.Add(item.ToUser);

            //    }
            //}
            if (volunteers.Count > 0)
            {
                foreach (var item in volunteers)
                {
                    if (!already_recommended.Contains(item))
                    {
                        all_volunteers.Add(item);
                    }
                }
            }
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
                Favorite_mission = favouritemission.Count,
                Avg_ratings = avg_ratings,
                Rating_count = rating_count,
                relatedMissions = related_mission,
                Applied_or_not = applied_or_not,
                Recent_volunteers = volunteers.Take(1).ToList(),
                Total_volunteers = volunteers.Count,
                documents = missiondocuments,
                All_volunteers = all_volunteers
            };
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
