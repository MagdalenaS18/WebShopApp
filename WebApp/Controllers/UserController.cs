using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.DTO;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationUserDto registrationUserDto)
        {
            return Ok(await _userService.Register(registrationUserDto));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            TokenDto token = await _userService.Login(loginUserDto);
            return Ok(token);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> ExternalLogin([FromForm] string googleToken)
        {
            TokenDto token = await _userService.ExternalLogin(googleToken);
            return Ok(token);
        }

        [HttpGet("user")]
        [Authorize(Roles = "ADMIN, SELLER, BUYER")]
        public async Task<IActionResult> GetUser()
        {
            UserDto userDto = await _userService.GetUser(_userService.GetUserIdFromToken(User));
            return Ok(userDto);
        }

        [HttpPut("edit-profile")]
        [Authorize(Roles = "ADMIN, SELLER, BUYER")]
        public async Task<IActionResult> EditProfile([FromForm] UpdateUserDto updateUserDto)
        {
            var editUserId = _userService.GetUserIdFromToken(User);
            return Ok(await _userService.UpdateUser(editUserId, updateUserDto));
        }

        [HttpPut("activation")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> SellerActivation([FromBody] ActivationUserDto activationUserDto)
        {
            return Ok(await _userService.ActivationUser(activationUserDto.Id, activationUserDto.IsActive));
        }

        [HttpGet("approved-sellers")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetActiveSellers()
        {
            return Ok(await _userService.GetActiveSellers());
        }

        [HttpGet("declined-sellers")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetUnactiveSellers()
        {
            return Ok(await _userService.GetUnactiveSellers());
        }

        [HttpPut("image-upload")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "ADMIN, SELLER, BUYER")]
        public async Task<IActionResult> ImageUpload(IFormFile file)
        {
            await _userService.UploadImage(_userService.GetUserIdFromToken(User), file);
            return Ok();
        }

        [HttpGet("image")]
        [Authorize(Roles = "ADMIN, SELLER, BUYER")]
        public async Task<IActionResult> GetImage()
        {
            UserImageDto imageDto = await _userService.GetUserImage(_userService.GetUserIdFromToken(User));
            return Ok(imageDto);
        }
    }
}
