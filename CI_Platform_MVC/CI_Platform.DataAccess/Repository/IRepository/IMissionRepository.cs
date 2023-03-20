using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository.IRepository
{
   public interface IMissionRepository : IRepository<Mission>
    {
        List<MissionViewModel> GetAllMission(int page , int pageSize);
        List<MissionViewModel> GetFilteredMissions(List<string> countriesList,List<string> cities, List<string> themes, List<string> skiils, string? sortOrder,int page, int pageSize);
        List<City> GetCitiesForCountry(long countryid);

      
        List<MissionSkill> MissionSkillList(int id);
        VolunteeringMissionVM GetMissionById(int id, long user_id);
        IEnumerable<CommentViewModel> comment(long user_id, long mission_id, string comment);
        bool add_to_favourite(long user_id, long mission_id);

        bool Rate_mission(long user_id, long mission_id, int rating);
        bool apply_for_mission(long user_id, long mission_id);
        bool Recommend(long user_id, long mission_id, List<long> co_workers);
        VolunteeringMissionVM Next_Volunteers(int count, long user_id, long mission_id);

    }
}
