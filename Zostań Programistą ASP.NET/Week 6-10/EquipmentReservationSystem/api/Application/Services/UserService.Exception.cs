using Application.Abstract;
using static Application.Abstract.ServiceException;

namespace Application.Services
{
    public partial class UserService
    {
        public abstract class UserServiceException : ServiceException
        {
            public UserServiceException(string message, Exception innerException = null) : base(message, innerException)
            {
                this.Occurred = nameof(UserService);
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
