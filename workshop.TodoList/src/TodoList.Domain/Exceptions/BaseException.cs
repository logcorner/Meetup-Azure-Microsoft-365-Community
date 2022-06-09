using System;

namespace TodoList.Domain.Exceptions
{
    public class BaseException : Exception
    {
        public int ErrorCode { get; }

        protected BaseException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}