using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Wernher.API.Validation;
using Wernher.Data.Context;
using Wernher.Data.Repositories;
using Wernher.Domain.Models;
using Wernher.Domain.Repositories;
using Wernher.Domain.Services;

namespace Wernher.API;

public static class ConfigureApi
{
    static private void AddSwaggerDoc(this IServiceCollection services)
        => services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "Wernher API Desafio Backend", Version = "v1" });
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {{ jwtSecurityScheme, Array.Empty<string>() }});
            });

    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WernherContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("WernherContext"))
                .EnableDetailedErrors()
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),
            ServiceLifetime.Scoped);

        var JWTkey = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);

        services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(JWTkey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
            });

        services.AddAuthorization(auth =>
            auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().Build()));


        services.AddSwaggerDoc();
        services.AddControllers();

        services.AddScoped<IValidator<Device>, DeviceValidator>();
        services.AddScoped<IValidator<Customer>, CustomerValidator>();

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IDeviceRepository, DeviceRepository>();

        services.AddScoped<AuthenticationService>();
        return services;
    }

    public static WebApplication ConfigureApp(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v2/swagger.json", "Wernher API V1"));
        app.UseStaticFiles();

        app.UseRouting();
        // app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();
        return app;
    }


}
