using System;

namespace UserManagementSystem.BLL.Exceptions
{
    public class AppException : Exception
    {
        public ExceptionType ExceptionType { get; }

        public AppException(ExceptionType exceptionType)
        {
            ExceptionType = exceptionType;
        }
    }
}
