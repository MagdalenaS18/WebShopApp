using System.ComponentModel.DataAnnotations;
using WebApp.Models.Enums;

namespace WebApp.DTO
{
    public class RegistrationUserDto
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public UserType type { get; set; }
    }
}
