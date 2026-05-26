using CourseProject.Domain.Models;

namespace CourseProject.Domain.Interfaces
{
    public interface IStorage
    {
        void Save(List<Advertisement> advertisements);

        List<Advertisement> Load();
    }
}