using Application.Abstract;
using Domain.Utils;

namespace Application.Services.PickupPoint
{
    public partial class ReservationPickupPointService
    {
        public class ReservationPickupPointServiceException : ServiceException
        {
            public ReservationPickupPointServiceException(string message, Exception innerException = null) : base(message, innerException)
            {
                this.Occurred = nameof(ReservationPickupPointService);
            }
        }

        public class IncorrectOperationException : ReservationPickupPointServiceException
        {
            public IncorrectOperationException(Exception innerException = null) : base("This operation is incorrect.", innerException)
            {
                this.TypeAction = ExceptionTypeAction.Argument;
            }
        }

        public class AdminAccessException : ReservationPickupPointServiceException
        {
            public AdminAccessException(Exception innerException = null) : base("The user does not have admin permissions.", innerException)
            {
                this.TypeAction = ExceptionTypeAction.NotAccess;
            }
        }

        public class InnerNoteLimitException : ReservationPickupPointServiceException
        {
            public InnerNoteLimitException(Exception innerException = null) : base("The internal note has reached its character limit.", innerException)
            {
                this.TypeAction = ExceptionTypeAction.Argument;
            }
        }

        public class ReservationChangeStatusException : ReservationPickupPointServiceException
        {
            public ReservationChangeStatusException(ReservationStatus newStatus, ReservationStatus oldStatus, Exception innerException = null) 
                : base($"Changing the reservation status from {oldStatus} to {newStatus} is not allowed.", innerException)
            {
                this.TypeAction = ExceptionTypeAction.Argument;
            }
        }
    }
}
