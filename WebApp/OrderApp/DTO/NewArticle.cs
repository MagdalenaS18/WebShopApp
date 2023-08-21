namespace OrderApp.DTO
{
    public class NewArticle
    {
        public long SellerId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}
