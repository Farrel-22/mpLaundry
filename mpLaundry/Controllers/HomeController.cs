using Microsoft.AspNetCore.Mvc;
using mpLaundry.Services.Interfaces;

namespace mpLaundry.Controllers;

public class HomeController(ILaundryService laundryService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var vm = await laundryService.GetDashboardAsync();
        return View(vm);
    }
}
