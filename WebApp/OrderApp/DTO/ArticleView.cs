namespace OrderApp.DTO
{
    public class ArticleView
    {
        public long Id { get; set; }
        public long SellerId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
