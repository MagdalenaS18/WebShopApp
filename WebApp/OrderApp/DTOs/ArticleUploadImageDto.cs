namespace OrderApp.DTOs
{
    public class ArticleUploadImageDto
    {
        public long Id { get; set; }
        public IFormFile File { get; set; }
    }
}
