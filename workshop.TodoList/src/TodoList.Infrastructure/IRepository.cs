using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Domain;
using TodoList.SharedKernel.Repository;

namespace TodoList.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task<Todo> Get(object id);

        Task<IEnumerable<T>> Get();

        Task Create(T entity);

        Task Delete(object id);

        Task Update(T entity);

        Task<PaginationItems<T>> Get(PaginationParameter paginationParameter);
    }
}