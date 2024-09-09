using Application.Abstract;

namespace Application.Services.User
{
    public partial class ItemUserService
    {
        public class ItemUserServiceException : AppSystemException
        {
            public ItemUserServiceException(string message, Exception innerException = null) : base(message, innerException)
            {
                this.Occurred = nameof(ItemUserService);
            }
        }

        public class ItemInstanceNotAvailableException : ItemUserServiceException
        {
            public ItemInstanceNotAvailableException(Exception innerException = null) : base("This item currently has no items available.", innerException)
            {
                this.TypeAction = ExceptionTypeAction.NotFound;
            }
        }
    }
}
