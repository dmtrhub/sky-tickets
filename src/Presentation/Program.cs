using Application;
using Infrastructure;
using Presentation.Extensions;
using System.Reflection;
using Web.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(builder.Configuration);
    
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.OpenApiWithScalar();

    app.ApplyMigrations();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

await app.RunAsync();

