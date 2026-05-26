using CourseProject.Domain.Enums;

namespace CourseProject.Domain.Models
{
    public class Admin : User
    {
        public Admin()
        {
            Role = UserRole.Admin;
        }

        public Admin(
            int id,
            string username,
            string password,
            string email,
            string name,
            string phoneNumber)
            : base(id, username, password, email, name, phoneNumber)
        {
            Role = UserRole.Admin;
        }

        public override bool Login(string username, string password)
        {
            return base.Login(username, password);
        }

        public void BlockUser(User user)
        {
        }

        public void DeleteAdvertisement(Advertisement advertisement)
        {
        }
    }
}