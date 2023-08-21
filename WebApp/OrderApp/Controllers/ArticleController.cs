using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApp.DTO;
using OrderApp.DTOs;
using OrderApp.Services.Interfaces;

namespace OrderApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IOrderService _orderService;

        public ArticleController(IArticleService articleService, IOrderService orderService)
        {
            _articleService = articleService;
            _orderService = orderService;
        }

        [HttpPost("add-article")]
        [Authorize(Roles = "SELLER")]
        public async Task<IActionResult> AddNewArticle([FromForm] ArticleDto articleDto)
        {
            //newArticle.SellerId = User.FindFirst("UserId").Value;
            //_articleService.GetUserIdFromToken(User);
            //await _articleService.UploadImage(_articleService.GetUserIdFromToken(User), file);
            return Ok(await _articleService.AddNewArticle(articleDto, _articleService.GetUserIdFromToken(User)));
        }

        [HttpPut("edit-article")]
        [Authorize(Roles = "SELLER")]
        public async Task<IActionResult> EditArticle([FromBody] UpdateArticleDto updateArticleDto)
        {
            return Ok(await _articleService.UpdateArticle(updateArticleDto.Id, updateArticleDto));
        }

        [HttpPut("delete")]
        [Authorize(Roles = "SELLER")]
        public async Task<IActionResult> DeleteArticle([FromForm] long id)
        {
            return Ok(await _articleService.DeleteArticle(id));
        }

        [HttpGet("all-articles")]
        [Authorize(Roles = "ADMIN, BUYER")]
        public async Task<IActionResult> GetAllArticles()
        {
            return Ok(await _articleService.GetAllArticles());
        }

        [HttpGet("article")]
        [Authorize]
        // bilo [FromBody]
        public async Task<IActionResult> GetArticle(long id)
        {
            return Ok(await _articleService.GetArticle(id, _articleService.GetUserIdFromToken(User)));
        }

        [HttpGet("article-image")]
        [Authorize(Roles = "ADMIN, SELLER, BUYER")]
        public async Task<IActionResult> GetArticleImage(long id)
        {
            //ArticleImageDto articleImageDto = await _articleService.GetArticleImage(id);
            return Ok(await _articleService.GetArticleImage(id));
        }

        [HttpGet("seller-articles")]
        [Authorize(Roles = "SELLER")]
        public async Task<IActionResult> GetSellerArticles()
        {
            return Ok(await _articleService.GetSellerArticles(_articleService.GetUserIdFromToken(User)));
        }

        [HttpPut("article-image")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "SELLER")]
        public async Task<IActionResult> UploadImage([FromForm] ArticleUploadImageDto articleUploadImageDto)
        {
            await _articleService.UploadImage(articleUploadImageDto.Id, articleUploadImageDto.File);
            return Ok();
        }
    }
}
