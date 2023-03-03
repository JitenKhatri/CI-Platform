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
        List<MissionViewModel> GetFilteredMissions(List<string> countries);

    }
}
