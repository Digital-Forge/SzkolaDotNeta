﻿namespace Application.Abstract
{
    public abstract class AppSystemException : Exception
    {
        public string Description { get; }
        public ExceptionTypeAction TypeAction { get; set; } = ExceptionTypeAction.Argument;
        public string Occurred { get; set; }

        public AppSystemException(string message, Exception innerException = null) : base(message, innerException)
        {
        }

        public enum ExceptionTypeAction
        {
            Argument,     // BadRequest
            Unauthoryze,  // Unautorize
            NotFound,     // Not Found
            NotAccess,    // Forbriden
            Error     // INTERNAL SERVER ERROR
        }
    }
}
