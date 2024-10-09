namespace ApplicationServices.Customers.Response
{
    public class TokenResponse
    {
        public required string AccessToken { get; set; }
        public string TokenType { get; set; } = "Bearer";
        public long ExpiresIn { get; set; }
    }
}