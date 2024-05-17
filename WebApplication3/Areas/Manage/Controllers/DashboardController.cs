using Microsoft.AspNetCore.Mvc;

namespace WebApplication3.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class DashboardController : Controller
    {
      
        public IActionResult Index()
        {
            return View();
        }
    }
}
