using System.ComponentModel.DataAnnotations;

namespace BiddingSystem.Application.DTOs.UserDtos
{
    public class UserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}




