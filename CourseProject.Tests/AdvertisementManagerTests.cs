using CourseProject.Domain.Models;
using CourseProject.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CourseProject.Tests
{
    [TestClass]
    public class AdvertisementManagerTests
    {
        private Category _category = null!;
        private RegisteredUser _user = null!;

        [TestInitialize]
        public void Setup()
        {
            _category = new Category(1, "Електроніка");
            _user = new RegisteredUser(1, "user", "password", "user@gmail.com");
        }

        [TestMethod]
        public void Add_ShouldAddAdvertisement()
        {
            var manager = new AdvertisementManager();

            var advertisement = new Advertisement(
                1,
                "Телефон",
                "Samsung Galaxy",
                8500,
                "Харків",
                _category,
                _user);

            var result = manager.Add(advertisement);

            Assert.IsTrue(result);
            Assert.AreEqual(1, manager.GetAll().Count);
        }

        [TestMethod]
        public void Add_ShouldReturnFalse_WhenIdAlreadyExists()
        {
            var manager = new AdvertisementManager();

            var ad1 = new Advertisement(1, "Телефон", "Опис", 8500, "Харків", _category, _user);
            var ad2 = new Advertisement(1, "Ноутбук", "Опис", 15000, "Київ", _category, _user);

            manager.Add(ad1);
            var result = manager.Add(ad2);

            Assert.IsFalse(result);
            Assert.AreEqual(1, manager.GetAll().Count);
        }

        [TestMethod]
        public void Delete_ShouldMarkAdvertisementAsDeleted()
        {
            var manager = new AdvertisementManager();

            var advertisement = new Advertisement(
                1,
                "Телефон",
                "Samsung Galaxy",
                8500,
                "Харків",
                _category,
                _user);

            manager.Add(advertisement);

            var result = manager.Delete(1, _user);

            Assert.IsTrue(result);
            Assert.AreEqual(0, manager.GetAll().Count);
        }

        [TestMethod]
        public void Search_ShouldReturnAdvertisementsByKeyword()
        {
            var manager = new AdvertisementManager();

            manager.Add(new Advertisement(1, "Телефон Samsung", "Опис", 8500, "Харків", _category, _user));
            manager.Add(new Advertisement(2, "Ноутбук Lenovo", "Опис", 15000, "Київ", _category, _user));

            var filter = new SearchFilter
            {
                Keyword = "Samsung"
            };

            var result = manager.Search(filter);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Телефон Samsung", result[0].Title);
        }

        [TestMethod]
        public void Search_ShouldReturnAdvertisementsByPriceRange()
        {
            var manager = new AdvertisementManager();

            manager.Add(new Advertisement(1, "Телефон", "Опис", 8500, "Харків", _category, _user));
            manager.Add(new Advertisement(2, "Ноутбук", "Опис", 15000, "Київ", _category, _user));

            var filter = new SearchFilter
            {
                MinPrice = 8000,
                MaxPrice = 9000
            };

            var result = manager.Search(filter);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Телефон", result[0].Title);
        }
    }
}
