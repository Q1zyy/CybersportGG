using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {   
        
        public IActionResult Index()
        {
            ViewBag.Name = User.Identity.Name;
            ViewBag.IsAuth = User.Identity.IsAuthenticated;
            ViewBag.Role = User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
            return View();
        }
        
        public IActionResult News()
        {
            return View();
        }
    }
}