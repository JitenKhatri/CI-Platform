using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CI_Platform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork db;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork _db)
        {
            _logger = logger;
            db = _db;
        }

        public IActionResult Index()
        {
            //return View();
            return RedirectToAction("login", "UserAuthentication");
        }
        

        public IActionResult Privacy()
        {
            var cmspages = db.AdminRepository.GetAllCmsPages();
            return View(cmspages);
        }
       
      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}