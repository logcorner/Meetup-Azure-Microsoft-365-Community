using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Domain;
using TodoList.SharedKernel.Repository;

namespace TodoList.Infrastructure
{
    public interface ITodoRepository
    {
        Task<Todo> Get(int id);

        Task<IEnumerable<Todo>> Get();

        Task Create(Todo todo);

        Task Delete(int id);

        Task Update(Todo todo);

        Task<PaginationItems<Todo>> Get(PaginationParameter paginationParameter);
    }
}