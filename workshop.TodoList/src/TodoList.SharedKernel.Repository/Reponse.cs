namespace TodoList.SharedKernel.Repository
{
    public class Reponse<T> where T : class
    {
        public PaginationItems<T> Data { get; set; }
        public PaginationItem PaginationItem { get; set; }
    }
}