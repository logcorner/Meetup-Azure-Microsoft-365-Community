using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Domain;
using TodoList.Infrastructure.Model;
using TodoList.SharedKernel.Repository;

namespace TodoList.Infrastructure
{
    public class TodoRepository : IRepository<Todo>
    {
        private readonly WorkshopdbContext _workshopdbContext;

        public TodoRepository(WorkshopdbContext workshopdbContext)
        {
            _workshopdbContext = workshopdbContext;
        }

        public async Task<Todo> Get(object id)
        {
            var result = _workshopdbContext.ToDo.Find(id);
            if (result == null)
            {
                return null;
            }
            var todo = new Todo
                (result.Id, result.Title, result.Description, result.ImageUrl);

            return await Task.FromResult(todo);
        }

        public async Task<IEnumerable<Todo>> Get()
        {
            var result = _workshopdbContext.ToDo.ToList();
            var todoes = result.Select(r => new Todo
                (r.Id, r.Title, r.Description, r.ImageUrl));
            return await Task.FromResult(todoes);
        }

        public async Task Create(Todo todo)
        {
            var item = new ToDo
            {
                Title = todo.Title,
                Description = todo.Description,
                ImageUrl = todo.ImageUrl,
                Status = todo.Status.GetIntValue()
            };

            _workshopdbContext.ToDo.Add(item);
            await _workshopdbContext.SaveChangesAsync();
        }

        public async Task Update(Todo todo)
        {
            var result = _workshopdbContext.ToDo.Find(todo.Id);

            if (result == null)
            {
                throw new Exception($"todo with id = {todo.Id} ");
            }
            result.Title = todo.Title;
            result.Description = todo.Description;
            result.Status = todo.Status.GetIntValue();
            result.ImageUrl = todo.ImageUrl;

            _workshopdbContext.Update(result);
            await _workshopdbContext.SaveChangesAsync();
        }

        public async Task Delete(object id)
        {
            var result = _workshopdbContext.ToDo.Find(id);
            if (result == null)
            {
                throw new Exception($"todo with id = {id} ");
            }
            _workshopdbContext.Remove(result);

            await _workshopdbContext.SaveChangesAsync();
        }

        public async Task<PaginationItems<Todo>> Get(PaginationParameter paginationParameter)
        {
            var todoes = _workshopdbContext.ToDo.Select(r => new Todo
                (r.Id, r.Title, r.Description, r.ImageUrl)).ToList();
            var result = PaginationItems<Todo>.ToPagedList(todoes.AsQueryable().OrderBy(on => on.Id),
                paginationParameter.PageNumber,
                paginationParameter.PageSize);

            return await Task.FromResult(result);
        }
    }
}