namespace TodoList.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message) : base(ErrorCodeConstant.NotFoundException, message)
        {
        }
    }
}