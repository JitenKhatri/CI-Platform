using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository.IRepository
{
    public interface IStoryRepository : IRepository<Story>
    {
        List<StoryViewModel> GetAllStories(int page, int pageSize);
        List<StoryViewModel> GetFilteredStories(List<string> Countries, List<string> Cities, List<string> Themes, List<string> Skills, string searchtext, int page = 1, int pageSize = 6 );
        List<City> CityCascade(long countryid);

        List<Mission> GetMissionApplications(long user_id);
    }
}
