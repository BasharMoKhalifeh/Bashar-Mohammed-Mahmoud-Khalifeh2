using System.ComponentModel.DataAnnotations;

namespace BiddingSystem.Application.DTOs.UserDtos
{
    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string NewPassword { get; set; }
    }
}




