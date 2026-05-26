using CourseProject.Domain.Models;

namespace CourseProject.Domain.Interfaces
{
    public interface ISearchable
    {
        List<Advertisement> Search(SearchFilter filter);
    }
}