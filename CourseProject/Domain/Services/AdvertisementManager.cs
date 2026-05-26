using CourseProject.Domain.Enums;
using CourseProject.Domain.Interfaces;
using CourseProject.Domain.Models;

namespace CourseProject.Domain.Services
{
    public class AdvertisementManager : ISearchable
    {
        private readonly List<Advertisement> _advertisements;

        public event Action<Advertisement>? AdvertisementCreated;
        public event Action<Advertisement>? AdvertisementDeleted;

        public AdvertisementManager()
        {
            _advertisements = new List<Advertisement>();
        }

        public List<Advertisement> GetAll()
        {
            return _advertisements
                .Where(ad => ad.Status != AdvertisementStatus.Deleted)
                .ToList();
        }

        public Advertisement? GetById(int id)
        {
            return _advertisements
                .FirstOrDefault(ad => ad.Id == id && ad.Status != AdvertisementStatus.Deleted);
        }

        public bool Add(Advertisement advertisement)
        {
            if (advertisement == null)
                throw new ArgumentNullException(nameof(advertisement));

            if (_advertisements.Any(ad => ad.Id == advertisement.Id))
                return false;

            _advertisements.Add(advertisement);
            AdvertisementCreated?.Invoke(advertisement);

            return true;
        }

        public bool Update(int id, Advertisement updatedAdvertisement, User currentUser)
        {
            var existingAdvertisement = GetById(id);

            if (existingAdvertisement == null)
                return false;

            if (existingAdvertisement.Author.Id != currentUser.Id && currentUser.Role != UserRole.Admin)
                return false;

            existingAdvertisement.Title = updatedAdvertisement.Title;
            existingAdvertisement.Description = updatedAdvertisement.Description;
            existingAdvertisement.Price = updatedAdvertisement.Price;
            existingAdvertisement.City = updatedAdvertisement.City;
            existingAdvertisement.Category = updatedAdvertisement.Category;

            return true;
        }

        public bool Delete(int id, User currentUser)
        {
            var advertisement = GetById(id);

            if (advertisement == null)
                return false;

            if (advertisement.Author.Id != currentUser.Id && currentUser.Role != UserRole.Admin)
                return false;

            advertisement.MarkAsDeleted();
            AdvertisementDeleted?.Invoke(advertisement);

            return true;
        }

        public List<Advertisement> Search(SearchFilter filter)
        {
            if (filter == null)
                return GetAll();

            if (!filter.IsValidPriceRange())
                return new List<Advertisement>();

            var query = GetAll().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                query = query.Where(ad =>
                    ad.Title.Contains(filter.Keyword, StringComparison.OrdinalIgnoreCase) ||
                    ad.Description.Contains(filter.Keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(ad => ad.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(ad => ad.Price <= filter.MaxPrice.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.City))
            {
                query = query.Where(ad =>
                    ad.City.Contains(filter.City, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.Category != null)
            {
                query = query.Where(ad => ad.Category.Id == filter.Category.Id);
            }

            return query
                .OrderByDescending(ad => ad.CreatedAt)
                .ToList();
        }
    }
}
