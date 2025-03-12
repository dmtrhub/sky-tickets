using Scalar.AspNetCore;

namespace Presentation.Extensions;

public static class ApplicationBuilderExtenstions
{
    public static IApplicationBuilder OpenApiWithScalar(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.Title = "Sky Tickets";
            options.Theme = ScalarTheme.DeepSpace;
            options.Layout = ScalarLayout.Modern;
        });

        return app;
    }
}
