using Application.Abstract;

namespace Application.Services
{
    public partial class FileService
    {
        public abstract class FileServiceException : ServiceException
        {
            public FileServiceException(string message, Exception innerException = null) : base(message, innerException)
            {
                this.Occurred = nameof(FileService);
            }
        }

        public class FileNotFoundException : FileServiceException
        {
            public FileNotFoundException(Exception innerException = null) : base("Not found file", innerException) 
            {
                this.TypeAction = ExceptionTypeAction.NotFound;
            }
        }
    }
}
