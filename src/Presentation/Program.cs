using Infrastructure;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.OpenApiWithScalar();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();

