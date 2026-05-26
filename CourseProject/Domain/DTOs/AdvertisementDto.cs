namespace CourseProject.Domain.DTOs
{
    public class AdvertisementDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string City { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string AuthorUsername { get; set; } = string.Empty;

        public List<string> ImagePaths { get; set; }

        public AdvertisementDto()
        {
            ImagePaths = new List<string>();
        }
    }
}