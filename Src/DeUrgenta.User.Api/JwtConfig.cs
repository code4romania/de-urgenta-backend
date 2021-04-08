namespace DeUrgenta.User.Api.Options
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public int TokenExpirationInSeconds { get; set; }
    }
}