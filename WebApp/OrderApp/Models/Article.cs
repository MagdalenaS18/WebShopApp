namespace OrderApp.Models
{
    public class Article
    {
        //public long Id { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public float Price { get; set; }
        //public int Quantity { get; set; }
        //public byte[] Image { get; set; }
        //public long SellerId { get; set; }
        ////public User Seller { get; set; }
        public List<OrderArticle> OrderArticles { get; set; }
        //public List<Order> Orders { get; set; }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public long SellerId { get; set; }
        public int MaxQuantity { get; set; }
        public byte[] PhotoUrl { get; set; }
    }
}
