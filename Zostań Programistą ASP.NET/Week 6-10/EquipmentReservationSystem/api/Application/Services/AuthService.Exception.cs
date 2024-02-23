
using Application.Abstract;
using static Application.Abstract.ServiceException;

namespace Application.Services
{
    public partial class AuthService
    {
        public abstract class AuthServiceException : ServiceException
        {
            public AuthServiceException(string message, Exception innerException = null) : base(message, innerException)
            {
                this.Occurred = nameof(AuthService);
            }
        }

        public class RefreshTokenExpiredException : AuthServiceException
        {
            public RefreshTokenExpiredException(Exception innerException = null) : base("Refresh token expired", innerException)
            {
                this.TypeAction = ExceptionTypeAction.Unauthoryze;
            }
        }
    }
}
