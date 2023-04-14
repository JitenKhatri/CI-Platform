using CI_Platform.Areas.Admin.ViewModels;
using CI_Platform.Models;
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

        Skill AddSkill(string SkillName, int Status);
        Skill EditSkill(int skillid, int Status, string SkillName);
        bool DeleteSkill(int skill_id);
        CrudViewModel GetAllMissionApplications();
        bool ApproveMissionApplication(int MissionApplicationId);
        bool DeclineMissionApplication(int MissionApplicationId);
        bool PublishStory(int StoryId);
        bool DeclineStory(int StoryId);
    }
}
