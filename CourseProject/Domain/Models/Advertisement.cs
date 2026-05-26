using CourseProject.Domain.Enums;

namespace CourseProject.Domain.Models
{
    public class Advertisement
    {
        private string _title = string.Empty;
        private string _description = string.Empty;
        private decimal _price;

        public int Id { get; set; }

        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Title cannot be empty");

                _title = value;
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Description cannot be empty");

                _description = value;
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative");

                _price = value;
            }
        }

        public string City { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public AdvertisementStatus Status { get; set; } = AdvertisementStatus.Active;

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public int AuthorId { get; set; }

        public RegisteredUser Author { get; set; } = null!;

        public List<AdvertisementImage> Images { get; set; } = new();

        public Advertisement()
        {
        }

        public Advertisement(
            int id,
            string title,
            string description,
            decimal price,
            string city,
            Category category,
            RegisteredUser author)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be empty");

            Id = id;
            Title = title;
            Description = description;
            Price = price;
            City = city;
            Category = category ?? throw new ArgumentNullException(nameof(category));
            Author = author ?? throw new ArgumentNullException(nameof(author));
            CreatedAt = DateTime.Now;
            Status = AdvertisementStatus.Active;
            Images = new List<AdvertisementImage>();
        }

        public void AddImage(AdvertisementImage image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            Images.Add(image);
        }

        public void MarkAsDeleted()
        {
            Status = AdvertisementStatus.Deleted;
        }

        public bool IsActive()
        {
            return Status == AdvertisementStatus.Active;
        }
    }
}