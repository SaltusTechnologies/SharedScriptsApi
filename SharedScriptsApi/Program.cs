using Asp.Versioning;
using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.Mvc;
using SharedScriptsApi.Data;
using SharedScriptsApi.Extensions;
using SharedScriptsApi.Interfaces;
using SharedScriptsApi.Services;


var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<SharedScriptsApiContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("SharedScriptsApiContext") ?? throw new InvalidOperationException("Connection string 'SharedScriptsApiContext' not found.")));
var services = builder.Services;
//services.AddTransient<AntiforgeryCookieResultFilter>();
// https://github.com/dotnet/aspnet-api-versioning/wiki
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddMvc(options => {
// https://github.com/dotnet/aspnet-api-versioning/wiki/API-Version-Conventions
options.Conventions.Controller<ControllerBase>()
    .HasApiVersions(
    // represents an api set
    [
        new ApiVersion(2.0)//,
                           //new ApiVersion(2.1)
    ]);
options.Conventions.Controller<ControllerBase>()
    .HasApiVersions(
    [
        new ApiVersion(2.0)
    ]);
});
//options.Conventions.Controller<V2_1Controller>()
//    .HasApiVersions([
//        new ApiVersion(2.1)
//    ]);
//}).AddApiExplorer(options =>
//options =>
//{
//    options.GroupNameFormat = "'v'VVV";
//    options.SubstituteApiVersionInUrl = true;
//    options.RouteConstraintName = "version";
//});

// Add services to the container.
services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

services.AddServices();

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
