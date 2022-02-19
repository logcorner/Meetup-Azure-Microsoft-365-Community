using Microsoft.Extensions.DependencyInjection;
using TodoList.Application;
using TodoList.Domain;
using TodoList.Infrastructure;

namespace TodoList.WebApi
{
    public static class ServicesConfiguration
    {
        public static void AddToDoServices(this IServiceCollection services)
        {
            services.AddScoped<ITodoUseCase, TodoUseCase>();
            services.AddScoped<IRepository<Todo>, TodoRepository>();
        }
    }
}