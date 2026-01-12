using Microsoft.AspNetCore.Mvc;

namespace WebApplication_sixteen_clothing.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
