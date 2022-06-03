using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoList.Infrastructure.Model;
using TodoList.WebApi;
using TodoList.WebApi.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var configuration = builder.Configuration;
builder.Services.AddDbContext<WorkshopdbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddToDoServices();

var app = builder.Build();

string pathBase = configuration["pathBase"];
app.UseSwagger(x =>
{
    if (!string.IsNullOrWhiteSpace(pathBase))
    {
        x.RouteTemplate = "swagger/{documentName}/swagger.json";
        x.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
        {
            swaggerDoc.Servers = new List<OpenApiServer>
                {new OpenApiServer {Url = $"https://{httpReq.Host.Value}{pathBase}"}};
        });
    }
});
app.UseSwaggerUI();

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (!string.IsNullOrWhiteSpace(pathBase))
{
    app.UsePathBase(new PathString(pathBase));
}

app.Run();