namespace CourseProject.Domain.Models
{
    public class Category
    {
        private string _name = string.Empty;

        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Category name cannot be empty");

                _name = value;
            }
        }

        public string Description { get; set; } = string.Empty;

        public List<Advertisement> Advertisements { get; set; } = new();

        public Category()
        {
        }

        public Category(int id, string name, string description = "")
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}