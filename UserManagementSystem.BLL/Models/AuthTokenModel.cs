namespace AuthorizationService.Models
{
    public class AuthTokenModel
    {
        public string Token { get; set; }

        public long ExpiredIn { get; set; }
    
        public string RefreshToken { get; set; }
    }
}
