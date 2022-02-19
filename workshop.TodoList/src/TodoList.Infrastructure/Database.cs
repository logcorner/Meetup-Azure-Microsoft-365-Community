//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using TodoList.Domain;
//using TodoList.SharedKernel.Repository;

//namespace TodoList.Infrastructure
//{
//    public class Database : IDatabase<Todo>
//    {
//        private readonly List<Todo> _todos = new List<Todo>();

//        public async Task CommitAsync()
//        {
//            await Task.CompletedTask;
//        }

//        public async Task<Todo> Get(object id)
//        {
//            var result = _todos.FirstOrDefault(s => s.Id == (int)id);
//            return await Task.FromResult(result);
//        }

//        public async Task<IEnumerable<Todo>> Get()
//        {
//            return await Task.FromResult(_todos);
//        }

//        public async Task Create(Todo todo)
//        {
//            //Increment identifier on database level
//            var id = _todos.Count == 0 ? 1 : _todos.Max(i => i.Id) + 1;
//            var item = new Todo(id, todo.Title, todo.Description, todo.ImageUrl);
//            _todos.Add(item);
//            await Task.CompletedTask;
//        }

//        public async Task Delete(object id)
//        {
//            var todo = _todos.FirstOrDefault(t => t.Id == (int)id);
//            if (todo != null)
//            {
//                _todos.Remove(todo);
//            }
//            await Task.CompletedTask;
//        }

//        public async Task<PaginationItems<Todo>> Get(PaginationParameter paginationParameter)
//        {
//            var result = PaginationItems<Todo>.ToPagedList(_todos.AsQueryable().OrderBy(on => on.Id),
//                paginationParameter.PageNumber,
//                paginationParameter.PageSize);

//            return await Task.FromResult(result);
//        }

//        public async Task Update(Todo todo)
//        {
//            var result = _todos.FirstOrDefault(t => t.Id == todo.Id);
//            if (result != null)
//            {
//                result = todo;
//            }
//            await Task.FromResult(result);
//        }
//    }
//}