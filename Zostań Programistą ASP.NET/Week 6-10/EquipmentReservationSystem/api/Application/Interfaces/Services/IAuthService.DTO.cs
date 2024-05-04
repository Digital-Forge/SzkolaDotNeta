namespace Application.Interfaces
{
    public partial interface IAuthService
    {
        class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        class TokenModel
        {
            public string Token { get; set; }
            public string RefreshToken { get; set; }
            public DateTime Expiry { get; set; }
        }
    }
}
