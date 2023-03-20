using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Controllers
{
    public class StoryController : Controller
    {
        private readonly IAllRepository db;
        public StoryController(IAllRepository _db)
        {
            db = _db;
        }
        public IActionResult Story(int page = 1, int pageSize = 6)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<StoryViewModel> stories = db.StoryRepository.GetAllStories(page, pageSize);
                return View(stories);
            }
            else
            {
                return RedirectToAction("login", "UserAuthentication");
            }
        }
    }
}
