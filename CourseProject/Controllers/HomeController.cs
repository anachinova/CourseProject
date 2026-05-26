using CourseProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string? keyword,
            string? categoryName,
            string? city,
            decimal? minPrice,
            decimal? maxPrice,
            string? sortBy)
        {
            var query = _context.Advertisements
                .Include(a => a.Category)
                .Include(a => a.Images)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var words = keyword
                    .Trim()
                    .ToLower()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    var searchPattern = $"%{word}%";

                    query = query.Where(a =>
                        EF.Functions.Like(a.Title.ToLower(), searchPattern) ||
                        EF.Functions.Like(a.Description.ToLower(), searchPattern));
                }
            }

            if (!string.IsNullOrWhiteSpace(categoryName) &&
                categoryName != "Усі категорії")
            {
                query = query.Where(a => a.Category.Name == categoryName);
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(a => a.City.Contains(city));
            }

            var advertisements = await query.ToListAsync();

            if (minPrice.HasValue)
            {
                advertisements = advertisements
                    .Where(a => a.Price >= minPrice.Value)
                    .ToList();
            }

            if (maxPrice.HasValue)
            {
                advertisements = advertisements
                    .Where(a => a.Price <= maxPrice.Value)
                    .ToList();
            }

            advertisements = sortBy switch
            {
                "price_asc" => advertisements
                    .OrderBy(a => a.Price)
                    .ToList(),

                "price_desc" => advertisements
                    .OrderByDescending(a => a.Price)
                    .ToList(),

                _ => advertisements
                    .OrderByDescending(a => a.CreatedAt)
                    .ToList()
            };

            ViewBag.Keyword = keyword;
            ViewBag.CategoryName = categoryName;
            ViewBag.City = city;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SortBy = sortBy;
            ViewBag.Categories = await _context.Categories
                .OrderBy(c => c.Name)
                .Select(c => c.Name)
                .ToListAsync();

            return View(advertisements);
        }
    }
}