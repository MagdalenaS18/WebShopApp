using AutoMapper;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserApp.DTO;
using UserApp.Models.Enums;
using UserApp.Models;
using UserApp.Repositories.Interfaces;
using UserApp.Services.Interfaces;

namespace UserApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IConfigurationSection _secretKey;
        private readonly IConfigurationSection _googleClientId;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration,
            IEmailService emailService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _secretKey = configuration.GetSection("SecretKey");
            _googleClientId = configuration.GetSection("GoogleClientId");
            _mapper = mapper;
        }

        public async Task<UserDto> ActivationUser(long id, bool activated)
        {
            var user = await _unitOfWork.Users.GetById(id);

            if (user == null)
            {
                throw new Exception("User doesn't exist.");
            }

            if (activated)
            {
                user.Approved = true;
                var email = user.Email;
                var message = new Email(new string[]
                  {$"{email}"},
                  "Profile activation",
                   "Your profile is active now!"
                );

                await _emailService.SendEmail(message);
            }
            else
            {
                user.Denied = true;
                var email = user.Email;
                var message = new Email(new string[]
                    {$"{email}" },
                    "Profile activation",
                    "Your registration has been denied."
                );

                await _emailService.SendEmail(message);
            }

            _unitOfWork.Users.UpdateUser(user);
            await _unitOfWork.Save();
            return _mapper.Map<UserDto>(user);
        }

        public Task ChangePassword(long id, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public async Task<TokenDto> ExternalLogin(string token)
        {
            ExternalUserDto externalUser = await VerifyGoogleToken(token);

            if (externalUser == null)
            {
                throw new Exception("Invalid Google token");
            }

            List<User> users = await _unitOfWork.Users.GetAll();
            User user = users.Find(u => u.Email == externalUser.Email);

            if (user == null)
            {
                user = new User()
                {
                    FullName = externalUser.Name,
                    Username = externalUser.UserName,
                    Email = externalUser.Email,
                    ProfilePictureUrl = new byte[0],
                    Password = "",
                    Address = "",
                    BirthDate = DateTime.Now,
                    Type = UserType.BUYER,
                    Approved = true
                };

                await _unitOfWork.Users.InsertUser(user);
                await _unitOfWork.Save();
            }

            List<Claim> userClaims = new List<Claim>();
            userClaims.Add(new Claim(ClaimTypes.Role, "BUYER"));
            userClaims.Add(new Claim(ClaimTypes.Name, user.Id.ToString()));

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey.Value));
            var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:44304",
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: signinCredentials
                );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenDto()
            {
                Token = tokenString,
                Role = user.Type.ToString(),
                UserId = user.Id
            };
        }

        public async Task<List<UserDto>> GetActiveSellers()
        {
            List<User> sellers = await _unitOfWork.Users.GetAll();
            return _mapper.Map<List<UserDto>>(
                sellers.Where(u => u.Type == UserType.SELLER && u.Approved == true).ToList());
        }

        public async Task<List<UserDto>> GetSellers()
        {
            List<User> sellers = await _unitOfWork.Users.GetAll();
            return _mapper.Map<List<UserDto>>(
                sellers.Where(u => u.Type == UserType.SELLER).ToList());
        }

        public async Task<List<UserDto>> GetUnactiveSellers()
        {
            List<User> sellers = await _unitOfWork.Users.GetAll();
            return _mapper.Map<List<UserDto>>(sellers.Where(u => u.Approved == false
                && u.Denied == false && u.Type == UserType.SELLER).ToList());
        }

        public async Task<UserDto> GetUser(long id)
        {
            var user = await _unitOfWork.Users.GetById(id);

            if (user == null)
            {
                throw new Exception("User doesn't exist.");
            }

            return _mapper.Map<UserDto>(user);
        }

        public long GetUserIdFromToken(ClaimsPrincipal user)
        {
            long id;
            long.TryParse(user.Identity.Name, out id);
            return id;
        }

        public async Task<UserImageDto> GetUserImage(long id)
        {
            var user = await _unitOfWork.Users.GetById((id));
            if (user == null)
            {
                throw new Exception("User doesn't exist.");
            }
            byte[] imageBytes = await _unitOfWork.Users.GetUserImage(id);

            UserImageDto usersImage = new UserImageDto()
            {
                ImageBytes = imageBytes
            };

            return usersImage;
        }

        public async Task<TokenDto> Login(LoginUserDto user)
        {
            List<User> users = await _unitOfWork.Users.GetAll();
            User userExists = users.Find(u => u.Email == user.Email);

            if (userExists == null)
            {
                throw new Exception($"User with email '{user.Email}' doesn't exist.");
            }

            if (BCrypt.Net.BCrypt.Verify(user.Password, userExists.Password))
            {
                List<Claim> userClaims = new List<Claim>();

                if (userExists.Type.ToString() == "ADMIN")
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, "ADMIN"));
                }
                if (userExists.Type.ToString() == "SELLER")
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, "SELLER"));
                }
                if (userExists.Type.ToString() == "BUYER")
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, "BUYER"));
                }

                userClaims.Add(new Claim(ClaimTypes.Name, userExists.Id.ToString()));

                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey.Value));
                var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var tokenOptions = new JwtSecurityToken(
                    issuer: "https://localhost:44304",
                    claims: userClaims,
                    expires: DateTime.Now.AddMinutes(20),
                    signingCredentials: signinCredentials
                    );
                string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return new TokenDto()
                {
                    Token = tokenString,
                    Role = userExists.Type.ToString(),
                    UserId = userExists.Id
                };
            }
            else
                throw new Exception("Password doesn't match. Try again.");
        }

        public async Task<UserDto> Register(RegistrationUserDto user)
        {
            List<User> users = await _unitOfWork.Users.GetAll();
            User userExists = users.Find(u => u.Email == user.Email);
            if (userExists != null)
            {
                throw new Exception($"User with Email: {user.Email} already exists.");
            }

            User newUser = _mapper.Map<User>(user);
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            if (newUser.Type.ToString() == "SELLER")
            {
                newUser.Approved = false;
            }
            else
            {
                newUser.Approved = true;
            }
            newUser.ProfilePictureUrl = new byte[0];
            newUser.Denied = false;
            await _unitOfWork.Users.InsertUser(newUser);
            await _unitOfWork.Save();

            return _mapper.Map<UserDto>(newUser);
        }

        public async Task<UpdateUserDto> UpdateUser(long id, UpdateUserDto user)
        {
            User userExists = await _unitOfWork.Users.GetById(id);
            if (userExists == null)
            {
                throw new Exception("User doesn't exist.");
            }

            var password = userExists.Password;
            var type = userExists.Type;
            var profilePic = userExists.ProfilePictureUrl;
            var approved = userExists.Approved;

            List<User> users = await _unitOfWork.Users.GetAll();
            User userEmailExists = users.Find(u => u.Email == user.Email && u.Id != id);
            if (userEmailExists != null)
            {
                throw new Exception($"Email '{user.Email} already in use.'");
            }

            userExists = _mapper.Map<User>(user);
            userExists.Password = password;
            userExists.Type = type;
            userExists.ProfilePictureUrl = profilePic;
            userExists.Id = id;
            userExists.Approved = approved;

            _unitOfWork.Users.UpdateUser(userExists);
            await _unitOfWork.Save();

            return _mapper.Map<UpdateUserDto>(userExists);
        }

        public async Task UploadImage(long id, IFormFile file)
        {
            var user = await _unitOfWork.Users.GetById(id);
            if (user == null)
            {
                throw new Exception("User doens't exist.");
            }

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();

                user.ProfilePictureUrl = fileBytes;
                _unitOfWork.Users.UpdateUser(user);
            }

            await _unitOfWork.Save();
        }

        private async Task<ExternalUserDto> VerifyGoogleToken(string externalLoginToken)
        {
            try
            {
                // google.apis.auth
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _googleClientId.Value }
                };

                var googleUserInfo = await GoogleJsonWebSignature.ValidateAsync(externalLoginToken, validationSettings);

                ExternalUserDto externalUser = new ExternalUserDto()
                {
                    UserName = googleUserInfo.Email.Split("@")[0],
                    Name = googleUserInfo.Name,
                    Email = googleUserInfo.Email
                };

                return externalUser;
            }
            catch
            {
                return null;
            }
        }
    }
}
