using Microsoft.AspNetCore.Mvc;

namespace mpLaundry.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
