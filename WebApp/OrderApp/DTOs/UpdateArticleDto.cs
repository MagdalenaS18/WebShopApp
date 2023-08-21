namespace OrderApp.DTOs
{
    public class UpdateArticleDto
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int MaxQuantity { get; set; }
    }
}
