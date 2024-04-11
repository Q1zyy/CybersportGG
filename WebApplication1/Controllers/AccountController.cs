using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {

        private readonly IUserService _userSevice;
        public AccountController(IUserService userService) {
            _userSevice = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        } 
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
            
        
        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user = _userSevice.GetUser(model.Username);

            if (user == null)
            {
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var claimIdentity = new ClaimsIdentity(claims, "Cookie");
            var claimPrincipal = new ClaimsPrincipal(claimIdentity);
            await HttpContext.SignInAsync("Cookie", claimPrincipal);

            ViewBag.Role = user.Role;
            return Redirect("/Home/Index");
        }

        [HttpPost]
        public IActionResult Register(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!_userSevice.HaveUser(model.Username))
            {
                _userSevice.AddUser(new Models.User()
                {
                    Username = model.Username,
                    Password = model.Password,
                    Role = "user"
                });
                return RedirectToAction("Login", "Account");

            }

            
            return View(model); 
        }

        public IActionResult Index()
        {
            return View();
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookie");
            return Redirect("/Home/Index");
        }

    }
}
