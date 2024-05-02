using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Authorize(Policy = "admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> Users()
        {
            var lst = await _userService.GetUsers();
            ViewBag.Users = lst;
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ChangeUserRole(string username, string newrole)
        {
            await _userService.ChangeRole(username, newrole);
            return Redirect("/admin/users");
        }

    }
}
