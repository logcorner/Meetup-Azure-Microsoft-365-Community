using System;

namespace TodoList.Application.Exceptions
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