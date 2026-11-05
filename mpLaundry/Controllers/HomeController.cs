using Microsoft.AspNetCore.Mvc;

namespace mpLaundry.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Message = "mpLaundry API is running" });
    }
}
