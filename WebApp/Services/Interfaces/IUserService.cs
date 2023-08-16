using System.Security.Claims;
using WebApp.DTO;

namespace WebApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> ActivationUser(long id, bool activated);
        Task ChangePassword(long id, string oldPassword, string newPassword);
        Task<TokenDto> ExternalLogin(string token);
        Task<List<UserDto>> GetUnactiveSellers();
        Task<List<UserDto>> GetActiveSellers();
        Task<List<UserDto>> GetSellers();
        Task<UserDto> GetUser(long id);
        long GetUserIdFromToken(ClaimsPrincipal user);
        Task<TokenDto> Login(LoginUserDto user);
        Task<UserDto> Register(RegistrationUserDto user);
        Task<UpdateUserDto> UpdateUser(long id, UpdateUserDto user);
        Task UploadImage(long id, IFormFile file);
        //Task<ExternalUserDto> VerifyGoogleToken(string externalLoginToken);
        //Task<UserImageDto> GetUserImage(long id);
    }
}
