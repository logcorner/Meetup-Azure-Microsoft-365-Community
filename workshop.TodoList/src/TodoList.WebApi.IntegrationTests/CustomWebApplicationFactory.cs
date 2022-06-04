using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using TodoList.Domain;
using TodoList.Infrastructure;

namespace TodoList.WebApi.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public FakeDataBase FakeDataBase { get; }

        public CustomWebApplicationFactory()
        {
            FakeDataBase = new FakeDataBase();
        }

        private IServiceProvider _serviceProvider;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IRepository<Todo>>(FakeDataBase);
                _serviceProvider = services.BuildServiceProvider();
            });
        }
    }
}