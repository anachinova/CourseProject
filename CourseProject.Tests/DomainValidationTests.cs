using CourseProject.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CourseProject.Tests
{
    [TestClass]
    public class DomainValidationTests
    {
        [TestMethod]
        public void User_ShouldThrowException_WhenPasswordIsTooShort()
        {
            Assert.Throws<ArgumentException>(() =>
                new RegisteredUser(1, "user", "123", "user@gmail.com"));
        }

        [TestMethod]
        public void User_ShouldThrowException_WhenEmailIsInvalid()
        {
            Assert.Throws<ArgumentException>(() =>
                new RegisteredUser(1, "user", "password", "invalid-email"));
        }

        [TestMethod]
        public void Advertisement_ShouldThrowException_WhenTitleIsEmpty()
        {
            var category = new Category(1, "Електроніка");
            var user = new RegisteredUser(1, "user", "password", "user@gmail.com");

            Assert.Throws<ArgumentException>(() =>
                new Advertisement(
                    1,
                    "",
                    "Опис",
                    8500,
                    "Харків",
                    category,
                    user));
        }

        [TestMethod]
        public void Advertisement_ShouldThrowException_WhenPriceIsNegative()
        {
            var category = new Category(1, "Електроніка");
            var user = new RegisteredUser(1, "user", "password", "user@gmail.com");

            Assert.Throws<ArgumentException>(() =>
                new Advertisement(
                    1,
                    "Телефон",
                    "Опис",
                    -100,
                    "Харків",
                    category,
                    user));
        }

        [TestMethod]
        public void SearchFilter_ShouldReturnFalse_WhenMinPriceGreaterThanMaxPrice()
        {
            var filter = new SearchFilter
            {
                MinPrice = 10000,
                MaxPrice = 5000
            };

            var result = filter.IsValidPriceRange();

            Assert.IsFalse(result);
        }
    }
}
