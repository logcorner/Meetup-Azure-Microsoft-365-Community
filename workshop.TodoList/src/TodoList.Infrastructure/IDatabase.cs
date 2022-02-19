/*using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Domain;
using TodoList.SharedKernel.Repository;

namespace TodoList.Infrastructure
{
    public interface IDatabase<T> where T :  class
    {
        Task CommitAsync();

        Task<Todo> Get(object id);

        Task<IEnumerable<T>> Get();

        Task Create(T entity);

        Task Update(T entity);

        Task Delete(object id);

        Task<PaginationItems<T>> Get(PaginationParameter paginationParameter);
    }
}*/