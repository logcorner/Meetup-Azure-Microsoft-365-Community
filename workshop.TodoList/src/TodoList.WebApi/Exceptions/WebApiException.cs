namespace TodoList.WebApi.Exceptions
{
    public class WebApiException : Exception
    {
        public WebApiException(string message) : base(message)
        {
        }
    }
}