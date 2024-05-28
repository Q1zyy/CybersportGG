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

            User user = await _userSevice.GetUser(model.Username);

            if (user == null || model.Password != user.Password)
            {
				ModelState.AddModelError("", "Неправильный логин или пароль.");
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
            return Redirect("/news");
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Password)
                || model.Password.Length < 8
                || !HasUppercase(model.Password)
                || !HasLowercase(model.Password)
                || !HasDigit(model.Password))
            {
                ModelState.AddModelError("", "Пароль должен" +
                    " быть длиной минимум 8 символов и содержать минимум одну заглавную букву," +
                    " одну строчную букву и одну цифру.");
                return View(model);
            }

            if (!(await _userSevice.HaveUser(model.Username)))
            {
                await _userSevice.AddUser(new Models.User()
                {
                    Username = model.Username,
                    Password = model.Password,
                    Role = "user"
                });
                TempData["SuccessMessage"] = "Вы успешно зарегистрированы.";
                return RedirectToAction("Login", "Account");

            }

            ModelState.AddModelError("", "Такой логин есть.");
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
            return Redirect("/News/News");
        }

        private bool HasUppercase(string password)
        {
            return password.Any(char.IsUpper);
        }

        private bool HasLowercase(string password)
        {
            return password.Any(char.IsLower);
        }

        private bool HasDigit(string password)
        {
            return password.Any(char.IsDigit);
        }

    }
}
