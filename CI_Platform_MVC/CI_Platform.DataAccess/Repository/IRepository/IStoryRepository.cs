using CI_Platform.Models;
using CI_Platform.Models.InputModels;
using CI_Platform.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository.IRepository
{
    public interface IStoryRepository : IRepository<Story>
    {
        (List<StoryViewModel>, int) GetAllStories(long user_id,int page, int pageSize);
        (List<StoryViewModel>, int) GetFilteredStories(StoryInputModel model);
        List<City> CityCascade(long countryid);

        List<Mission> GetMissionApplications(long user_id);

        bool ShareStory(StoryInputModel model);
        StoryViewModel GetStoryDetail(long user_id, long id);
        void Add_View(long user_id, long story_id);
        bool Recommend(long user_id, long story_id, long to_user_id);
    }
}
