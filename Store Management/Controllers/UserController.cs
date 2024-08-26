using Microsoft.AspNetCore.Mvc;

namespace Store_Management.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
