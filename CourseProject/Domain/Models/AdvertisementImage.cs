namespace CourseProject.Domain.Models
{
    public class AdvertisementImage
    {
        public int Id { get; set; }

        public string Path { get; set; } = string.Empty;

        public int AdvertisementId { get; set; }

        public Advertisement Advertisement { get; set; } = null!;

        public AdvertisementImage()
        {
        }

        public AdvertisementImage(int id, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Image path cannot be empty");

            Id = id;
            Path = path;
        }
    }
}