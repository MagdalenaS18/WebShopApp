namespace OrderApp.DTO
{
    public class UpdateArticle
    {
        public long SellerId { get; set; }
        public long ArticleId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public IFormFile UpdatedImage { get; set; }
    }
}
