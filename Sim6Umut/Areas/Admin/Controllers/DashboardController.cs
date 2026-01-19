using Microsoft.AspNetCore.Mvc;

namespace Sim6Umut.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
