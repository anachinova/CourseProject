using CourseProject.Domain.Enums;

namespace CourseProject.Domain.Models
{
    public abstract class User
    {
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _email = string.Empty;
        private string _name = string.Empty;
        private string _phoneNumber = string.Empty;

        public int Id { get; set; }

        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username cannot be empty");

                _username = value;
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 6)
                    throw new ArgumentException("Password must contain at least 6 characters");

                _password = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                    throw new ArgumentException("Invalid email");

                _email = value;
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty");

                _name = value;
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Phone number cannot be empty");

                _phoneNumber = value;
            }
        }

        public UserRole Role { get; protected set; }

        protected User()
        {
        }

        protected User(
            int id,
            string username,
            string password,
            string email,
            string name,
            string phoneNumber)
        {
            Id = id;
            Username = username;
            Password = password;
            Email = email;
            Name = name;
            PhoneNumber = phoneNumber;
        }

        public virtual bool Login(string username, string password)
        {
            return Username == username && Password == password;
        }

        public virtual void Logout()
        {
        }
    }
}