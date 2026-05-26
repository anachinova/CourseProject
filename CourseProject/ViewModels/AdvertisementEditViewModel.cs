using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourseProject.ViewModels
{
    public class AdvertisementEditViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string City { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();

        public List<AdvertisementImageViewModel> ExistingImages { get; set; } = new();

        public List<IFormFile>? NewImages { get; set; }
    }
}