using OrderApp.DTO;
using OrderApp.DTOs;
using OrderApp.Models;
using System.Security.Claims;

namespace OrderApp.Services.Interfaces
{
    public interface IArticleService
    {
        //Task<Article> AddNewArticle(NewArticle newArticle);
        //Task<List<ArticleView>> GetAll();
        //Task<List<ArticleView>> GetAllBySellerId(long sellerId);
        //Task<Article> Update(UpdateArticle article);
        //Task<string> Delete(DeleteArticle article);
        //Task<byte[]> ParseArticleImageToBytes(IFormFile incomingImage);
        //long GetUserIdFromToken(ClaimsPrincipal user);
        //Task UploadImage(long id, IFormFile file);

        Task<List<GetArticleDto>> GetAllArticles();
        Task<List<GetArticleDto>> GetSellerArticles(long id);
        Task<ArticleDto> GetArticle(long id, long userId);
        Task<ArticleDto> AddNewArticle(ArticleDto newArticle, long sellerId);
        Task<bool> DeleteArticle(long id);
        Task<UpdateArticleDto> UpdateArticle(long id, UpdateArticleDto newArticle);
        Task UploadImage(long id, IFormFile file);
        Task<ArticleImageDto> GetArticleImage(long id);
        long GetUserIdFromToken(ClaimsPrincipal user);
    }
}
