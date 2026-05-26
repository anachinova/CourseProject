using CourseProject.Domain.Models;
using CourseProject.Domain.Storage;

namespace CourseProject.Tests
{
    [TestClass]
    public class JsonStorageTests
    {
        private readonly string _testFilePath = "test_ads.json";

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [TestMethod]
        public void Save_ShouldCreateJsonFile()
        {
            var storage = new JsonStorage(_testFilePath);

            var category = new Category(1, "Електроніка");

            var user = new RegisteredUser(
                1,
                "user",
                "password",
                "user@gmail.com");

            var advertisements = new List<Advertisement>
            {
                new Advertisement(
                    1,
                    "Телефон",
                    "Samsung Galaxy",
                    8500,
                    "Харків",
                    category,
                    user)
            };

            storage.Save(advertisements);

            Assert.IsTrue(File.Exists(_testFilePath));
        }

        [TestMethod]
        public void Load_ShouldReturnAdvertisements()
        {
            var storage = new JsonStorage(_testFilePath);

            var category = new Category(1, "Електроніка");

            var user = new RegisteredUser(
                1,
                "user",
                "password",
                "user@gmail.com");

            var advertisements = new List<Advertisement>
            {
                new Advertisement(
                    1,
                    "Телефон",
                    "Samsung Galaxy",
                    8500,
                    "Харків",
                    category,
                    user)
            };

            storage.Save(advertisements);

            var loadedAdvertisements = storage.Load();

            Assert.AreEqual(1, loadedAdvertisements.Count);
            Assert.AreEqual("Телефон", loadedAdvertisements[0].Title);
        }

        [TestMethod]
        public void Load_ShouldReturnEmptyList_WhenFileDoesNotExist()
        {
            var storage = new JsonStorage("not_existing_file.json");

            var advertisements = storage.Load();

            Assert.AreEqual(0, advertisements.Count);
        }
    }
}
