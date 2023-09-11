namespace Metafar.Challange.Entities.Api.V1
{
    public class AuthResponse : BasicResponse
    {
        public AuthResponse(string status, Guid correlation, string token, string refreshToken, DateTime expiration)
            :base(status, correlation)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
            RefreshToken = refreshToken ?? throw new ArgumentNullException( nameof(refreshToken));
            ExpirationTime = expiration;
        }

        public string Token
        {
            get;
            set;
        }

        public string RefreshToken
        {
            get;
            set;
        }

        public DateTime ExpirationTime
        {
            get;
            set;
        }
    }
}
