namespace TodoList.Application.Exceptions
{
    public class ApplicationException : BaseException
    {
        public ApplicationException(int errorCode, string message) : base(errorCode, message)
        {
        }
    }
}