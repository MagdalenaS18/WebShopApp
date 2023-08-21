namespace OrderApp.DTOs
{
    public class ArticleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int MaxQuantity { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
