using AutoMapper;
using OrderApp.DTO;
using OrderApp.DTOs;
using OrderApp.Models;
using OrderApp.Repositories;
using OrderApp.Repositories.Interfaces;
using OrderApp.Services.Interfaces;
using System.Security.Claims;

namespace OrderApp.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public long GetUserIdFromToken(ClaimsPrincipal user)
        {
            long id;
            long.TryParse(user.Identity.Name, out id);
            return id;
        }

        public async Task<ArticleDto> AddNewArticle(ArticleDto newArticle, long sellerId)
        {
            List<Article> list = await _unitOfWork.Articles.GetAll();
            var article = list.Find(a => a.Name == newArticle.Name);
            if (article != null) { throw new Exception($"Article with Name: {newArticle.Name} already exists."); }

            //var seller = GetUserIdFromToken(user.Identity.Name);
            //if (seller == null) { throw new Exception("User doesn't exist."); }

            //if (!seller.Approved)
            //{
            //    throw new ConflictException("SELLER isn't approved. Wait for admin approval!");
            //}

            article = _mapper.Map<Article>(newArticle);

            using (var ms = new MemoryStream())
            {
                newArticle.FormFile.CopyTo(ms);
                var fileBytes = ms.ToArray();

                article.PhotoUrl = fileBytes;
                _unitOfWork.Articles.UpdateArticle(article);
            }
            article.SellerId = sellerId;
            await _unitOfWork.Articles.InsertArticle(article);
            await _unitOfWork.Save();
            return _mapper.Map<ArticleDto>(article);
        }

        public async Task<bool> DeleteArticle(long id)
        {
            var article = await _unitOfWork.Articles.GetById(id);
            if(article == null)
            {
                return false;
            }
            else
            {
                _unitOfWork.Articles.DeleteArticle(id);
                _unitOfWork.Save();
                return true;
            }
        }

        public async Task<List<GetArticleDto>> GetAllArticles()
        {
            return _mapper.Map<List<GetArticleDto>>(await _unitOfWork.Articles.GetAll());
        }

        public async Task<ArticleDto> GetArticle(long id, long userId)
        {
            var article = await _unitOfWork.Articles.GetById(id);
            if(article == null)
            {
                throw new Exception("Article doesn't exist.");
            }
            
            return _mapper.Map<ArticleDto>(article);
            //if (userId == article.SellerId)
            //{
                
            //}
            //else
            //{
            //    throw new Exception("Article not available.");
            //}

            //return _mapper.Map<ArticleDto>(article);
            //ClaimsPrincipal user;
            //user = ClaimsPrincipal.Current;
            //var userType = user.Identity.GetType();

            //if (user.IsInRole("BUYER"))
            //{
                //return _mapper.Map<ArticleDto>(article);
            //}
            //if(user.IsInRole("SELLER") && userId == article.SellerId)
            //{
            //    return _mapper.Map<ArticleDto>(article);
            //}
            //else
            //{
            //    throw new Exception("Article not available.");
            //}
        }

        public async Task<ArticleImageDto> GetArticleImage(long id)
        {
            var article = await _unitOfWork.Articles.GetById(id);
            if(article == null)
            {
                throw new Exception("Article doesn't exist.");
            }

            byte[] imageBytes = await _unitOfWork.Articles.GetArticleImage(id);

            ArticleImageDto articleImage = new ArticleImageDto()
            {
                ImageBytes = imageBytes
            };

            return articleImage;
        }

        public async Task<List<GetArticleDto>> GetSellerArticles(long id)
        {
            return _mapper.Map<List<GetArticleDto>>(await _unitOfWork.Articles.GetSellerArticles(id));
        }

        public async Task<UpdateArticleDto> UpdateArticle(long id, UpdateArticleDto newArticle)
        {
            var articleExists = await _unitOfWork.Articles.GetById(id);
            if(articleExists == null)
            {
                throw new Exception("Article doesn't exist.");
            }

            string name = articleExists.Name;
            long sellerId = articleExists.SellerId;
            var photo = articleExists.PhotoUrl;

            articleExists = _mapper.Map<Article>(newArticle);
            articleExists.Name = name;
            articleExists.SellerId = sellerId;
            articleExists.PhotoUrl = photo;

            _unitOfWork.Articles.UpdateArticle(articleExists);
            await _unitOfWork.Save();
            return _mapper.Map<UpdateArticleDto>(articleExists);
        }

        public async Task UploadImage(long id, IFormFile file)
        {
            var article = await _unitOfWork.Articles.GetById(id);
            if (article == null)
            {
                throw new Exception("Article doesn't exist.");
            }

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();

                article.PhotoUrl = fileBytes;
                _unitOfWork.Articles.UpdateArticle(article);
            }
            await _unitOfWork.Save();
        }
    }
}
