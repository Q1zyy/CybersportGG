using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Authorize(Policy = "writer")]
    public class AdminController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

    }
}
