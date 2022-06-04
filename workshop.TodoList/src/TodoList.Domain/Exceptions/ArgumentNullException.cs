namespace TodoList.Domain.Exceptions
{
    public class ArgumentNullException : DomainException
    {
        public ArgumentNullException(string message) : base(ErrorCodeConstant.ArgumentNullException, message)
        {
        }
    }
}