using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEWS_WebAplication.Data;
using NEWS_WebAplication.Models.Account;
using NEWS_WebAplication.Services;
using NewsAPI.Data;
using NewsAPI.Models;
using NuGet.Protocol;
using System.Data;
using System.Security.Claims;

namespace NEWS_WebAplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly NewsDbContext _newsDbContext;
        private readonly IMailSender _mailSender;
        public AccountController(NewsDbContext newsDbContext, IMailSender mailSender)
        {
            _newsDbContext = newsDbContext;
            _mailSender = mailSender;
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());

        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var user = _newsDbContext.Users.FirstOrDefault(a => a.Email == model.UserName);

            if (ModelState.IsValid && user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.password))
            {                        

                    var roles = _newsDbContext.UserRoles.Include(a =>a.role).Where(a => a.UserId == user.UserId);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.DisplayName),
                        new Claim("UserId", user.UserId.ToString())


                    };

                    foreach (var item in roles)
                    {
                     claims.Add(new Claim(ClaimTypes.Role, item.role.Name));
                    }
                    

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity), new AuthenticationProperties());

                return RedirectToAction("Index","Home");             
     
            }

            model.serErrorMessage("Username Or password is invalid");
            return View(model);

        }

        public IActionResult register()
        {
            return View(new RegisterViewModels());
        }

        [HttpPost]
        public IActionResult register(RegisterViewModels model)
        {
            if (ModelState.IsValid)
            {
                var user = new User();
                user.Email = model.Email;
                user.password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                user.DisplayName = model.DisplayName;

                _newsDbContext.Users.Add(user);
                _newsDbContext.SaveChanges();

                return RedirectToAction("Login");
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return LocalRedirect("/");

        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _newsDbContext.Users.FirstOrDefault(a => a.Email == model.Email);

                string token = Guid.NewGuid().ToString();

                var passwordreset = new PasswordReset();
                passwordreset.UserId = user.UserId;           
                passwordreset.CreatedAt = DateTime.UtcNow;
                passwordreset.ExpiredAt = DateTime.UtcNow.AddHours(24);
                passwordreset.token = token; 

                _newsDbContext.PasswordResets.Add(passwordreset);
                _newsDbContext.SaveChanges();

                var message = "<h1>Reset Password<h1/>";
                message += $"HEY, {user.DisplayName}.";
                message += $"Click The link To reset your password";
                message += $"<a href='{Request.Scheme}://{Request.Host}/{Url.Action("ResetPassword", new { token }).Trim('/')}'>Reset Password<a/>";

                await _mailSender.send(user.Email, "Password Reset", message); 
                return RedirectToAction("Login");
            }
            return View(model);
        }

        public IActionResult ResetPassword(string Token)
        {
            var Resetpassword = _newsDbContext.PasswordResets.Where(a => a.token == Token).FirstOrDefault();
            if (Resetpassword == null)
            {
                return RedirectToAction("Login");
            }

            if (DateTime.UtcNow > Resetpassword.ExpiredAt)
            {
                return RedirectToAction("Login");
            }

            var user = _newsDbContext.Users.Find(Resetpassword.UserId);

            var viewmodel = new PasswordResetViewModel();
            viewmodel.Email = user.Email;
            viewmodel.Token = Token;

            return View(viewmodel);
        }

        [HttpPost]
        public IActionResult ResetPassword(PasswordResetViewModel model)
        {
            var user = _newsDbContext.Users.FirstOrDefault(a=> a.Email == model.Email);
            var Resetpassword = _newsDbContext.PasswordResets.Where(a => a.token == model.Token)
                .Where(a => a.UserId == user.UserId)
                .FirstOrDefault();
            if (Resetpassword == null)
            {
                return RedirectToAction("Login");
            }

            if (DateTime.UtcNow > Resetpassword.ExpiredAt)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                user = _newsDbContext.Users.Find(Resetpassword.UserId);
                user.password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                _newsDbContext.SaveChanges();

                return RedirectToAction("Login", new { message= "PasswordResetok" });
            }
                
            return View(model);
        }
    }
}
