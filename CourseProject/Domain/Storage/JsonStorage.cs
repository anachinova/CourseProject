using System.Text.Json;
using CourseProject.Domain.DTOs;
using CourseProject.Domain.Interfaces;
using CourseProject.Domain.Models;

namespace CourseProject.Domain.Storage
{
    public class JsonStorage : IStorage
    {
        private readonly string _filePath;

        public JsonStorage(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be empty");

            _filePath = filePath;
        }

        public void Save(List<Advertisement> advertisements)
        {
            var dtoList = advertisements.Select(ad => new AdvertisementDto
            {
                Id = ad.Id,
                Title = ad.Title,
                Description = ad.Description,
                Price = ad.Price,
                City = ad.City,
                CreatedAt = ad.CreatedAt,
                CategoryName = ad.Category.Name,
                AuthorUsername = ad.Author.Username,
                ImagePaths = ad.Images
                    .Select(img => img.Path)
                    .ToList()
            }).ToList();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(dtoList, options);

            File.WriteAllText(_filePath, json);
        }

        public List<Advertisement> Load()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Advertisement>();
            }

            var json = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<Advertisement>();
            }

            var dtoList = JsonSerializer.Deserialize<List<AdvertisementDto>>(json);

            if (dtoList == null)
            {
                return new List<Advertisement>();
            }

            var advertisements = dtoList.Select(dto =>
            {
                var category = new Category(1, dto.CategoryName);

                var user = new RegisteredUser(
                    1,
                    dto.AuthorUsername,
                    "password",
                    "user@gmail.com",
                    dto.AuthorUsername,
                    "+380000000000");

                var advertisement = new Advertisement(
                    dto.Id,
                    dto.Title,
                    dto.Description,
                    dto.Price,
                    dto.City,
                    category,
                    user);

                foreach (var imagePath in dto.ImagePaths)
                {
                    advertisement.AddImage(
                        new AdvertisementImage(1, imagePath));
                }

                return advertisement;

            }).ToList();

            return advertisements;
        }
    }
}