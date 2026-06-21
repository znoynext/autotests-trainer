using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutotestsTrainer.Web.Controllers;

[Route("api-console")]
[Authorize]
public class ApiConsoleController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
