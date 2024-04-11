using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Authorize(Policy = "writer")]
    public class WriterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
