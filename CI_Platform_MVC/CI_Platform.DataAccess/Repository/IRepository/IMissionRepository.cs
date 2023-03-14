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
        List<MissionViewModel> GetAllMission();
        List<MissionViewModel> GetFilteredMissions(List<string> countriesList,List<string> cities, List<string> themes, List<string> skiils, string? sortOrder);
        List<City> GetCitiesForCountry(long countryid);

      
        List<MissionSkill> MissionSkillList(int id);
        VolunteeringMissionVM GetMissionById(int id);
        IEnumerable<CommentViewModel> comment(long user_id, long mission_id, string comment/*, int length*/);
    }
}
