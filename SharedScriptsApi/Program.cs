using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedScriptsApi.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SharedScriptsApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SharedScriptsApiContext") ?? throw new InvalidOperationException("Connection string 'SharedScriptsApiContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
