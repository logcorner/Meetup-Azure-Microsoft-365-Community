using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Application.Messages;
using TodoList.Domain;
using TodoList.SharedKernel.Repository;

namespace TodoList.Application
{
    public interface ITodoUseCase
    {
        Task<Todo> GetTodo(int id);

        Task<IEnumerable<Todo>> GetTodo();

        Task AddToDo(ToDoForCreationMessage toDoForCreationMessage);

        Task<PaginationItems<Todo>> GetTodo(PaginationParameter paginationParameter);

        Task UpdateToDo(ToDoForUpdateMessage toDoForUpdateMessage);

        Task DeleteToDo(int id);
    }
}