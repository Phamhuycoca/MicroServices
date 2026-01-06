using AuthService.API.Models;
using AuthService.Application.DTO;
using AuthService.Application.IServices;
using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AuthService.API.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<nguoi_dung> _signInManager;
        private readonly UserManager<nguoi_dung> _userManager;
        private readonly IMemoryCache _cache;
        private readonly IServiceRefreshToken _serviceRefreshToken;
        public AuthController(
           SignInManager<nguoi_dung> signInManager,
           UserManager<nguoi_dung> userManager,
           IMemoryCache cache,
           IServiceRefreshToken serviceRefreshToken
           )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _cache = cache;
            _serviceRefreshToken = serviceRefreshToken;
        }
        [HttpGet]
        public IActionResult Auth_Login()
        {
            /*if (User.Identity != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    return Redirect("http://localhost:5173/");
                }
            }*/
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
            if (await _userManager.IsLockedOutAsync(user))
            {
                ModelState.AddModelError("", "Tài khoản đã bị khóa, vui lòng thử lại sau");
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
                var maxAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts;
                var failedCount = await _userManager.GetAccessFailedCountAsync(user);
                var remain = maxAttempts - failedCount;

                if (remain > 0 && remain < 5)
                {
                    ModelState.AddModelError("", $"Sai tài khoản hoặc mật khẩu. Bạn còn {remain} lần thử.");
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản đã bị khóa do nhập sai quá nhiều lần");
                }

                return View(model);
            }
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var token = await _serviceRefreshToken.GenerateTokensAsync(user, ip);

            var oneTimeCode = Guid.NewGuid().ToString("N");

            // 🔥 KHÔNG await
            _cache.Set(
                oneTimeCode,
                token,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                }
            );
            return Redirect($"http://localhost:5173/login-success?code={oneTimeCode}");
        }
       
        [HttpGet]
        public IActionResult Auth_ForgotPassword()
        {
            if (User.Identity != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    return Redirect("http://localhost:5173/");
                }
            }
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
