using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;


namespace WebApplication_sixteen_clothing.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
           return View();
        }

    }
        
}
