using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;


namespace Sim6Umut.Controllers
{
    public class HomeController : Controller
    {
       

        public IActionResult Index()
        {
            return View();
        }

    }
}
