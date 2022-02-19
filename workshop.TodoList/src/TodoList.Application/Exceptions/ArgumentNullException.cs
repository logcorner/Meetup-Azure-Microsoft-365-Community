namespace TodoList.Application.Exceptions
{
    public class ArgumentNullException : ApplicationException
    {
        public ArgumentNullException(string message) : base(ErrorCodeConstant.ArgumentNullException, message)
        {
        }
    }
}