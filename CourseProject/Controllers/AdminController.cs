using CourseProject.Data;
using CourseProject.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.UsersCount = await _context.RegisteredUsers.CountAsync();
            ViewBag.AdvertisementsCount = await _context.Advertisements.CountAsync();
            ViewBag.CategoriesCount = await _context.Categories.CountAsync();
            ViewBag.ImagesCount = await _context.AdvertisementImages.CountAsync();

            var latestAdvertisements = await _context.Advertisements
                .Include(a => a.Category)
                .Include(a => a.Author)
                .OrderByDescending(a => a.CreatedAt)
                .Take(5)
                .ToListAsync();

            return View(latestAdvertisements);
        }

        [HttpGet]
        public async Task<IActionResult> Advertisements()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Auth");
            }

            var advertisements = await _context.Advertisements
                .Include(a => a.Category)
                .Include(a => a.Author)
                .Include(a => a.Images)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(advertisements);
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Auth");
            }

            var users = await _context.RegisteredUsers
                .Include(u => u.Advertisements)
                .OrderBy(u => u.Username)
                .ToListAsync();

            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAdvertisement(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Auth");
            }

            var advertisement = await _context.Advertisements
                .Include(a => a.Images)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (advertisement == null)
            {
                return NotFound();
            }

            foreach (var image in advertisement.Images)
            {
                var filePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    image.Path.TrimStart('/'));

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Advertisements.Remove(advertisement);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Оголошення успішно видалено адміністратором.";
            return RedirectToAction(nameof(Advertisements));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await _context.RegisteredUsers
                .Include(u => u.Advertisements)
                    .ThenInclude(a => a.Images)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            foreach (var advertisement in user.Advertisements)
            {
                foreach (var image in advertisement.Images)
                {
                    var filePath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        image.Path.TrimStart('/'));

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }

            _context.RegisteredUsers.Remove(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Користувача успішно видалено.";
            return RedirectToAction(nameof(Users));
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == UserRole.Admin.ToString();
        }
    }
}
