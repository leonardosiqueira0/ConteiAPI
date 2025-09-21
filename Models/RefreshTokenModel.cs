namespace ConteiAPI.Models
{
    public class RefreshTokenModel
    {
        public Guid? Id { get; set; } = Guid.NewGuid();
        public required string Token { get; set; }
        public required DateTime ExpirationDate { get; set; }
        public required bool IsRevoked { get; set; }
        public required Guid UserId { get; set; }

        
    }
}
