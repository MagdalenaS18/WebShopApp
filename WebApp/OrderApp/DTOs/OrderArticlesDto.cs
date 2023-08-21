namespace OrderApp.DTOs
{
    public class OrderArticlesDto
    {
        public long OrderId { get; set; }
        public List<OrderArticleDto> OrderArticles { get; set; }
    }
}
