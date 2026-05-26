using CourseProject.Domain.Enums;

namespace CourseProject.Domain.Models
{
    public class RegisteredUser : User
    {
        public List<Advertisement> Advertisements { get; set; } = new();

        public RegisteredUser()
        {
            Role = UserRole.RegisteredUser;
        }

        public RegisteredUser(
            int id,
            string username,
            string password,
            string email,
            string name,
            string phoneNumber)
            : base(id, username, password, email, name, phoneNumber)
        {
            Role = UserRole.RegisteredUser;
            Advertisements = new List<Advertisement>();
        }

        public void AddAdvertisement(Advertisement advertisement)
        {
            Advertisements.Add(advertisement);
        }

        public void RemoveAdvertisement(Advertisement advertisement)
        {
            Advertisements.Remove(advertisement);
        }
    }
}