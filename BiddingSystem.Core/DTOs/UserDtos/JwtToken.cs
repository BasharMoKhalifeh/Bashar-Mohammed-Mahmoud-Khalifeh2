namespace BiddingSystem.Application.DTOs.UserDtos
{
    public class JwtToken
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}




