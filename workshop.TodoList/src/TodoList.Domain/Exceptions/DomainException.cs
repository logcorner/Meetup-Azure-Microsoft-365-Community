namespace TodoList.Domain.Exceptions
{
    public class DomainException : BaseException
    {
        public DomainException(int errorCode, string message) : base(errorCode, message)
        {
        }
    }
}