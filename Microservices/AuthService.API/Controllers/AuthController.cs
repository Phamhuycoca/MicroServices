using AuthService.API.Models;
using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<nguoi_dung> _signInManager;
        private readonly UserManager<nguoi_dung> _userManager;
        public AuthController(
           SignInManager<nguoi_dung> signInManager,
           UserManager<nguoi_dung> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Auth_Login()
        {
            ViewBag.FormLogin = new FormLogin()
            {
                title = "Đăng nhập hệ thống",
                image = "https://images.unsplash.com/photo-1522202176988-66273c2fd55f"

            };
            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Auth_Login(LoginViewModel model)
        {
            ViewBag.FormLogin = new FormLogin()
            {
                title = "Đăng nhập hệ thống",
                image = "https://images.unsplash.com/photo-1522202176988-66273c2fd55f"

            };
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true
            );
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
                return View(model);
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Tài khoản đã bị khóa");
                return View(model);
            }

            return Redirect("http://localhost:5173/");
        }
        [HttpGet]
        public IActionResult Auth_ForgotPassword()
        {
            ViewBag.FormLogin = new FormLogin()
            {
                title = "Đăng nhập hệ thống",
                image = "https://images.unsplash.com/photo-1522202176988-66273c2fd55f"

            };
            return View(new LoginViewModel());
        }
        [HttpPost]
        public IActionResult Auth_ForgotPassword(LoginViewModel model)
        {
            ViewBag.FormLogin = new FormLogin()
            {
                title = "Đăng nhập hệ thống",
                image = "https://images.unsplash.com/photo-1522202176988-66273c2fd55f"

            };
            if (!ModelState.IsValid)
                return View(model);

            var result = false;

            if (!result)
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
                return View(model);
            }

            return Redirect("/");
        }
    }
}
