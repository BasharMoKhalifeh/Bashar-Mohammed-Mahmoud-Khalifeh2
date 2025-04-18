namespace BiddingSystem.Application.DTOs.UserDtos
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public JwtToken Token { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}




