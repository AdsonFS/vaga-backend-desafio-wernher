using System.Reflection;
using Elastic.Apm.SerilogEnricher;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Filters;
using Serilog.Sinks.Elasticsearch;

namespace Wernher.API;

public static class SerilogExtension
{
    public static void AddSerilogApi(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        var logLevel = builder.Environment.IsProduction() ? LogEventLevel.Information : LogEventLevel.Debug;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
            .Filter.ByExcluding(z => z.MessageTemplate.Text.Contains("Business error"))
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("ApplicationName", "Wernher API")
            .Enrich.WithElasticApmCorrelationInfo()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
            })

            .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss} {ElasticApmTraceId} {ElasticApmTransactionId} {ElasticApmSpanId} [{Level:u3}] [{SourceContext}] -> {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(
                path: $"logs/wernher api_",
                outputTemplate: "{Timestamp:HH:mm:ss} {ElasticApmTraceId} {ElasticApmTransactionId} {ElasticApmSpanId} [{Level:u3}] [{SourceContext}] -> {Message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Host.UseSerilog(Log.Logger, true);
    }
}