﻿using CI_Platform.Areas.Admin.ViewModels;
using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository.IRepository
{
    public interface IAdminRepository
    {
        void Save();
        MissionTheme AddTheme(string ThemeName, int status);
        MissionTheme EditTheme(int theme_id, int Status, string ThemeName);
        bool DeleteTheme(int theme_id);
        CrudViewModel GetAllUsers();
        CrudViewModel GetAllMissions();
        CrudViewModel GetAllThemes();
        CrudViewModel GetAllSkills();
        CrudViewModel GetAllStories();
        CrudViewModel GetAllCmsPages();
        Skill AddSkill(string SkillName, int Status);
        Skill EditSkill(int skillid, int Status, string SkillName);
        bool DeleteSkill(int skill_id);
        CrudViewModel GetAllMissionApplications();
        bool ApproveMissionApplication(int MissionApplicationId,string MissionLink);
        bool DeclineMissionApplication(int MissionApplicationId, string MissionLink);
        bool PublishStory(int StoryId,string StoryLink);
        bool DeclineStory(int StoryId,string StoryLink);
        bool DeleteStory(int StoryId);
        StoryViewModel GetStoryDetail(int StoryId);
        AddUserViewModel GetCitiesForCountries(int CountryId);
        List<Country> GetAllCountries();
        bool AddUser(AddUserViewModel model);
        bool UserExists(string email);
        bool DeleteUser(int UserId);
        List<City> GetAllCities();
        User GetUserById(int UserId);
        CmsPage GetCmsPageById(long CMSPageId);
        bool AddCmsPage(AddCMSViewModel addCMSViewModel);
        bool DeleteCMSPage(long CMSPageId);
        List<MissionTheme> GetAllMissionThemes();
        List<Skill> GetAllMissionSkills();
        long AddMission(AddMissionViewModel addMissionViewModel);
        AddMissionViewModel GetMissionById(long MissionId);
        bool DeleteMission(long MissionId);

        CrudViewModel GetAllBanners();

        bool Addbanner(AddBannerViewModel addBannerViewModel);
        AddBannerViewModel GetBannerById(int BannerId);
        bool DeleteBanner(int BannerId);
        IEnumerable<AddBannerViewModel> GetBanners();
        bool NotifyuserEmail(long userid);
        CrudViewModel GetAllTimesheets();
        bool ApproveTimesheet(int TimesheetId, string MissionLink);
        bool DeclineTimesheet(int TimesheetId, string MissionLink);
        long Getmissionidbytimesheetid(int timesheetid);
        long GetMissionidbymissionapplicationid(int missionapplicationid);
        CrudViewModel GetAllComments();
        bool PublishComment(int CommentId, string MissionLink);
        bool DeclineComment(int CommentId, string PolicyLink);
    }
}
