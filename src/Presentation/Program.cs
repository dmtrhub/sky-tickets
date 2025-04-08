using Application;
using Hangfire;
using Infrastructure;
using Presentation;
using Presentation.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services
    .AddApplication(builder.Configuration)
    .AddPresentation()
    .AddInfrastructure(builder.Configuration);
    
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.UseHangfireDashboard();
app.UseHangfireJobs();

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

