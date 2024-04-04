using FluentValidation;
using Wernher.API.DTO;
using Wernher.API.Validation;

namespace Wernher.API;

public static class ConfigureApi
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen();
        services.AddControllers();
        services.AddScoped<IValidator<DeviceDto>, DeviceValidator>();
        return services;
    }

    public static WebApplication ConfigureApp(this WebApplication app)
    {
        // todo -> optei pelo swagger em producao a fim de demonstracao
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseRouting();
        app.MapControllers();
        return app;
    }


}
