namespace CourseProject.Domain.Models
{
    public class SearchFilter
    {
        public string? Keyword { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public string? City { get; set; }

        public Category? Category { get; set; }

        public bool IsValidPriceRange()
        {
            if (MinPrice.HasValue && MaxPrice.HasValue)
            {
                return MinPrice <= MaxPrice;
            }

            return true;
        }
    }
}