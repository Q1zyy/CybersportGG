using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {

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

            string path = "D:\\Forum\\WebApplication1\\WebApplication1\\users.txt";

            bool skip = false;
            string currentRole = "user";

            using (StreamReader reader = new StreamReader(path))
            {
                string text = "";
                while (text != null)
                {
                    text = reader.ReadLine();
                    if (text == null) break;
                    var a = text.Split(' ');
                    if (a[0] == model.Username && a[1] == model.Password)
                    {
                        skip = true;
                        currentRole = a[2];
                        break;
                    }
                }
            }

            if (!skip)
            {
                return View(model);

            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Role, currentRole)
            };
            var claimIdentity = new ClaimsIdentity(claims, "Cookie");
            var claimPrincipal = new ClaimsPrincipal(claimIdentity);
            await HttpContext.SignInAsync("Cookie", claimPrincipal);

            ViewBag.Role = currentRole;
            return Redirect("/Home/Index");
        }

        [HttpPost]
        public IActionResult Register(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string path = "D:\\Forum\\WebApplication1\\WebApplication1\\users.txt";

            bool skip = false;

            using (StreamReader reader = new StreamReader(path))
            {
                string text = "";
                while (text !=  null)
                {
                    text = reader.ReadLine();
                    if (text == null) break;
                    var a = text.Split(' ');
                    if (a[0] == model.Username)
                    {
                        skip = true;
                    }
                }
                Console.WriteLine(text);
            }

            if (!skip)
            {
                using (StreamWriter reader = new StreamWriter(path, true))
                {

                    reader.WriteLine(model.Username + " " + model.Password + " user");
                }

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
