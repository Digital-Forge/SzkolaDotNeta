using Application.Abstract;

namespace Application.Services
{
    public partial class DepartmentAdminService
    {
        public class DepartmentServiceException : AppSystemException
        {
            public DepartmentServiceException(string message, Exception innerException = null) : base(message, innerException)
            {
                this.Occurred = nameof(DepartmentAdminService);
            }
        }

        public class NotFoundDepartment : DepartmentServiceException
        {
            public NotFoundDepartment(Exception innerException = null) : base("Can't find department", innerException)
            {
                this.TypeAction = ExceptionTypeAction.NotFound;
            }
        }
    }
}
