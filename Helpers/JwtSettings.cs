namespace ConteiAPI.Helpers
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int AccessTokenExpirationMinutes { get; set; } = 60; // Default to 1 hour
        public int RefreshTokenExpirationDays { get; set; } = 30; // Default to 30 days
    }
}
