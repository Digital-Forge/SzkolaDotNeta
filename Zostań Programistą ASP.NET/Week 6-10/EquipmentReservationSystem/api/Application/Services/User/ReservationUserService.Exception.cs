using Application.Abstract;

namespace Application.Services.User
{
    public partial class ReservationUserService
    {
        public class ReservationUserServiceException : ServiceException
        {
            public ReservationUserServiceException(string message, Exception innerException = null) : base(message, innerException)
            {
                this.Occurred = nameof(ReservationUserService);
            }
        }

        public class ReservationAccessDeniedException : ReservationUserServiceException
        {
            public ReservationAccessDeniedException(Exception innerException = null) : base("A user without the required permissions tried to access the reservation resources.", innerException)
            {
                this.TypeAction = ExceptionTypeAction.NotAccess;
            }
        }

        public class ItemInstanceNotAvailableException : ReservationUserServiceException
        {
            public ItemInstanceNotAvailableException(Exception innerException = null) : base("This item currently has no items available.", innerException)
            {
                this.TypeAction = ExceptionTypeAction.NotFound;
            }
        }

        public class NotFountItemException : ReservationUserServiceException
        {
            public NotFountItemException(Exception innerException = null) : base("Not found item", innerException)
            {
                this.TypeAction = ExceptionTypeAction.NotFound;
            }
        }
    }
}
