using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CourseProject.ViewModels
{
    public class AdvertisementCreateViewModel
    {
        [Required(ErrorMessage = "Введіть назву оголошення")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть опис")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть ціну")]
        [Range(0, 100000000, ErrorMessage = "Ціна має бути більше або дорівнювати 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Введіть місто")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Оберіть категорію")]
        public int CategoryId { get; set; }

        public List<IFormFile> Images { get; set; } = new();

        public List<SelectListItem> Categories { get; set; } = new();
    }
}