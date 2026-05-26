using CourseProject.Data;
using CourseProject.Domain.Models;
using CourseProject.Domain.Enums;
using CourseProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userExists = await _context.RegisteredUsers
                .AnyAsync(u => u.Username == model.Username || u.Email == model.Email);

            var adminExists = await _context.Admins
                .AnyAsync(a => a.Username == model.Username || a.Email == model.Email);

            if (userExists || adminExists)
            {
                ModelState.AddModelError("", "Користувач з таким логіном або email вже існує");
                return View(model);
            }

            var user = new RegisteredUser(
                0,
                model.Username,
                model.Password,
                model.Email,
                model.Name,
                model.PhoneNumber);

            _context.RegisteredUsers.Add(user);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserRole", UserRole.RegisteredUser.ToString());

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var admin = await _context.Admins
                .FirstOrDefaultAsync(a =>
                    (a.Username == model.Login || a.Email == model.Login)
                    && a.Password == model.Password);

            if (admin != null)
            {
                HttpContext.Session.SetInt32("AdminId", admin.Id);
                HttpContext.Session.SetString("Username", admin.Username);
                HttpContext.Session.SetString("UserName", admin.Name);
                HttpContext.Session.SetString("UserRole", UserRole.Admin.ToString());

                return RedirectToAction("Index", "Admin");
            }

            var user = await _context.RegisteredUsers
                .FirstOrDefaultAsync(u =>
                    (u.Username == model.Login || u.Email == model.Login)
                    && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Невірний логін/email або пароль");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserRole", UserRole.RegisteredUser.ToString());

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}