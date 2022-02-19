using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Application.Messages;
using TodoList.Domain;
using TodoList.Infrastructure;
using TodoList.SharedKernel.Repository;

namespace TodoList.Application
{
    public class TodoUseCase : ITodoUseCase
    {
        private IRepository<Todo> TodoRepository { get; }

        public TodoUseCase(IRepository<Todo> todoRepository)
        {
            TodoRepository = todoRepository;
        }

        public async Task<Todo> GetTodo(int id)
        {
            return await TodoRepository.Get(id);
        }

        public async Task<IEnumerable<Todo>> GetTodo()
        {
            return await TodoRepository.Get();
        }

        public async Task AddToDo(ToDoForCreationMessage toDoForCreationMessage)
        {
            if (toDoForCreationMessage == null)
            {
                throw new Exceptions.ArgumentNullException($"toDoForCreationMessage cannot be null {nameof(toDoForCreationMessage)}");
            }

            await TodoRepository.Create(new Todo(toDoForCreationMessage.Title,
                toDoForCreationMessage.Description, toDoForCreationMessage.ImageUrl));
        }

        public async Task UpdateToDo(ToDoForUpdateMessage toDoForUpdateMessage)
        {
            if (toDoForUpdateMessage == null)
            {
                throw new Exceptions.ArgumentNullException($"toDoForUpdateMessage cannot be null {nameof(toDoForUpdateMessage)}");
            }

            var todo = await TodoRepository.Get(toDoForUpdateMessage.Id);
            if (todo == null)
            {
                throw new Exceptions.NotFoundException($" todo with id = {toDoForUpdateMessage.Id} does not exist {nameof(todo)} ");
            }

            todo.Update(toDoForUpdateMessage.Title, toDoForUpdateMessage.Description, toDoForUpdateMessage.ImageUrl);
            await TodoRepository.Update(todo);
        }

        public async Task DeleteToDo(int id)
        {
            var todo = await TodoRepository.Get(id);
            if (todo == null)
            {
                throw new Exceptions.NotFoundException($" todo with id = {id} does not exist {nameof(todo)} ");
            }
            await TodoRepository.Delete(id);
        }

        public async Task<PaginationItems<Todo>> GetTodo(PaginationParameter paginationParameter)
        {
            var result = await TodoRepository.Get(paginationParameter);
            return result;
        }
    }
}