using CourseProject.Data;
using CourseProject.Domain.Models;
using CourseProject.Domain.Services;
using CourseProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Controllers
{
    public class AdvertisementsController : Controller
    {
        private readonly AdvertisementManager _advertisementManager;
        private readonly ApplicationDbContext _context;

        public AdvertisementsController(
            AdvertisementManager advertisementManager,
            ApplicationDbContext context)
        {
            _advertisementManager = advertisementManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> My()
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var advertisements = await _context.Advertisements
                .Include(a => a.Category)
                .Include(a => a.Images)
                .Where(a => a.AuthorId == currentUser.Id)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(advertisements);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var advertisement = await _context.Advertisements
                .Include(a => a.Category)
                .Include(a => a.Author)
                .Include(a => a.Images)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (advertisement == null)
            {
                return NotFound();
            }

            return View(advertisement);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = new AdvertisementCreateViewModel
            {
                Categories = await GetCategoriesAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdvertisementCreateViewModel model)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (model.Images != null && model.Images.Count > 10)
            {
                ModelState.AddModelError("Images", "Можна завантажити максимум 10 фотографій.");
            }

            if (model.Images != null)
            {
                foreach (var image in model.Images)
                {
                    if (image.Length == 0)
                    {
                        continue;
                    }

                    var extension = Path.GetExtension(image.FileName).ToLower();
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };

                    if (!image.ContentType.StartsWith("image/") || !allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("Images", "Можна завантажувати тільки фотографії: jpg, jpeg, png, webp, gif.");
                        break;
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategoriesAsync();
                return View(model);
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == model.CategoryId);

            if (category == null)
            {
                ModelState.AddModelError("CategoryId", "Оберіть коректну категорію");
                model.Categories = await GetCategoriesAsync();
                return View(model);
            }

            var newId = _advertisementManager.GetAll().Any()
                ? _advertisementManager.GetAll().Max(ad => ad.Id) + 1
                : 1;

            var advertisement = new Advertisement(
                0,
                model.Title,
                model.Description,
                model.Price,
                model.City,
                category,
                currentUser
            );

            advertisement.CategoryId = category.Id;
            advertisement.AuthorId = currentUser.Id;

            if (model.Images != null && model.Images.Any())
            {
                var uploadFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads",
                    "advertisements"
                );

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                foreach (var image in model.Images.Take(10))
                {
                    if (image.Length == 0)
                    {
                        continue;
                    }

                    var extension = Path.GetExtension(image.FileName).ToLower();
                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    var imagePath = $"/uploads/advertisements/{fileName}";

                    advertisement.Images.Add(new AdvertisementImage
                    {
                        Path = imagePath,
                        Advertisement = advertisement
                    });
                }
            }

            _context.Advertisements.Add(advertisement);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Оголошення успішно створено.";
            return RedirectToAction("My");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var advertisement = await _context.Advertisements
                .Include(a => a.Category)
                .Include(a => a.Images)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (advertisement == null)
            {
                return NotFound();
            }

            if (advertisement.AuthorId != currentUser.Id)
            {
                return Forbid();
            }

            var model = new AdvertisementEditViewModel
            {
                Id = advertisement.Id,
                Title = advertisement.Title,
                Description = advertisement.Description,
                Price = advertisement.Price,
                City = advertisement.City,
                CategoryId = advertisement.CategoryId,
                Categories = await GetCategoriesAsync(),
                ExistingImages = advertisement.Images
                    .Select(img => new AdvertisementImageViewModel
                    {
                        Id = img.Id,
                        Path = img.Path
                    })
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdvertisementEditViewModel model)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategoriesAsync();
                return View(model);
            }

            var advertisement = await _context.Advertisements
                .Include(a => a.Images)
                .FirstOrDefaultAsync(a => a.Id == model.Id);

            if (advertisement == null)
            {
                return NotFound();
            }

            if (advertisement.AuthorId != currentUser.Id)
            {
                return Forbid();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == model.CategoryId);

            if (category == null)
            {
                ModelState.AddModelError("CategoryId", "Оберіть коректну категорію");
                model.Categories = await GetCategoriesAsync();
                return View(model);
            }

            advertisement.Title = model.Title;
            advertisement.Description = model.Description;
            advertisement.Price = model.Price;
            advertisement.City = model.City;
            advertisement.CategoryId = category.Id;
            advertisement.Category = category;

            if (model.NewImages != null && model.NewImages.Any())
            {
                var currentImagesCount = advertisement.Images.Count;
                var availableSlots = 10 - currentImagesCount;

                if (availableSlots <= 0)
                {
                    ModelState.AddModelError("NewImages", "Можна мати максимум 10 фотографій.");
                    model.Categories = await GetCategoriesAsync();
                    model.ExistingImages = advertisement.Images
                        .Select(img => new AdvertisementImageViewModel
                        {
                            Id = img.Id,
                            Path = img.Path
                        })
                        .ToList();

                    return View(model);
                }

                foreach (var image in model.NewImages.Take(availableSlots))
                {
                    if (image.Length == 0)
                    {
                        continue;
                    }

                    var extension = Path.GetExtension(image.FileName).ToLower();
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };

                    if (!image.ContentType.StartsWith("image/") || !allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("NewImages", "Можна завантажувати тільки фотографії: jpg, jpeg, png, webp, gif.");
                        model.Categories = await GetCategoriesAsync();
                        model.ExistingImages = advertisement.Images
                            .Select(img => new AdvertisementImageViewModel
                            {
                                Id = img.Id,
                                Path = img.Path
                            })
                            .ToList();

                        return View(model);
                    }

                    var uploadFolder = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "uploads",
                        "advertisements"
                    );

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    advertisement.Images.Add(new AdvertisementImage
                    {
                        Path = $"/uploads/advertisements/{fileName}",
                        AdvertisementId = advertisement.Id
                    });
                }
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Оголошення успішно оновлено.";
            return RedirectToAction("My");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(int imageId, int advertisementId)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var image = await _context.AdvertisementImages
                .Include(i => i.Advertisement)
                .FirstOrDefaultAsync(i => i.Id == imageId && i.AdvertisementId == advertisementId);

            if (image == null)
            {
                return NotFound();
            }

            if (image.Advertisement.AuthorId != currentUser.Id)
            {
                return Forbid();
            }

            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                image.Path.TrimStart('/')
            );

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.AdvertisementImages.Remove(image);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Фото успішно видалено.";
            return RedirectToAction("Edit", new { id = advertisementId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
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

            if (advertisement.AuthorId != currentUser.Id)
            {
                return Forbid();
            }

            _context.Advertisements.Remove(advertisement);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Оголошення успішно видалено.";
            return RedirectToAction("My");
        }

        private async Task<RegisteredUser?> GetCurrentUserAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return null;
            }

            return await _context.RegisteredUsers
                .FirstOrDefaultAsync(u => u.Id == userId.Value);
        }

        private async Task<List<SelectListItem>> GetCategoriesAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();
        }
    }
}