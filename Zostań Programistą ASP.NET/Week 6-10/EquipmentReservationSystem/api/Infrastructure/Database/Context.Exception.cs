using Application.Abstract;

namespace Infrastructure.Database
{
    public partial class Context
    {
        public abstract class ContextException : AppSystemException
        {
            public ContextException(string message, Exception innerException = null) : base(message, innerException)
            {
                this.Occurred = nameof(Context);
            }
        }

        public class UserUnrecognizedException : ContextException
        {
            public UserUnrecognizedException(Exception innerException = null) : base("The user required for the operation cannot be determined", innerException)
            {
                this.TypeAction = ExceptionTypeAction.Error;
            }
        }
    }
}
