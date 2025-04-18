using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiddingSystem.Infrastructure.Identity.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(UserRegistrationDto request);
        Task<AuthResult> LoginAsync(UserLoginDto request);
        Task<AuthResult> RefreshTokenAsync(string token, string refreshToken);
        Task<bool> ResetPasswordAsync(ResetPasswordDto request);
    }

}
