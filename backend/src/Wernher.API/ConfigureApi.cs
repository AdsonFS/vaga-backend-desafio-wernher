using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Wernher.API.Validation;
using Wernher.Data.Context;
using Wernher.Data.Repositories;
using Wernher.Domain.Models;
using Wernher.Domain.Repositories;

namespace Wernher.API;

public static class ConfigureApi
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WernherContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("WernherContext"))
                .EnableDetailedErrors()
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),
            ServiceLifetime.Scoped);


        services.AddSwaggerGen();
        services.AddControllers();
        services.AddScoped<IValidator<Device>, DeviceValidator>();
        services.AddScoped<IRepository<Device>, DeviceRepository>();
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
