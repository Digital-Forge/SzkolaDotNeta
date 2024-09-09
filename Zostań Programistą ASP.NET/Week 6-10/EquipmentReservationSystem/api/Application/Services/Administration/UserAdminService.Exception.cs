using Application.Abstract;
using static Application.Abstract.AppSystemException;

namespace Application.Services
{
    public partial class UserAdminService
    {
        public abstract class UserServiceException : AppSystemException
        {
            public UserServiceException(string message, Exception innerException = null) : base(message, innerException)
            {
                this.Occurred = nameof(UserAdminService);
            }
        }

        public class UserNotFoundException : UserServiceException
        {
            public UserNotFoundException(Exception innerException = null) : base("Not found user", innerException)
            {
                this.TypeAction = ExceptionTypeAction.NotFound;
            }
        }

        public class UserCreateException : UserServiceException
        {
            public UserCreateException(Exception innerException = null) : base("Can't create user", innerException)
            {
                this.TypeAction = ExceptionTypeAction.Argument;
            }
        }
    }
}
