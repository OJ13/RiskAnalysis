using Microsoft.OpenApi;
using RiskAnalisys.API.Middleware;
using RiskAnalisys.Application.DI;
using RiskAnalisys.Application.Serialization;
using System.Text.Json.Serialization;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

#region Controller Config

builder.Services.AddControllers().AddJsonOptions(
    options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        options.JsonSerializerOptions.TypeInfoResolver = RiskAnalisysJsonContext.Default;
    }
).ConfigureApiBehaviorOptions(
    options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

#endregion 

# region Injecao de Dependencias

builder.Services.AddDIApplication();

#endregion

# region OpenTelemetry Configuration

const string serviceName = "risk-analisys";

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName))
            .AddConsoleExporter()
            .AddOtlpExporter();
});

builder.Services.AddOpenTelemetry()
      .ConfigureResource(resource => resource.AddService(serviceName))
      .WithTracing(tracing => tracing
          .AddAspNetCoreInstrumentation()
          .AddConsoleExporter())
      .WithMetrics(metrics => metrics
          .AddAspNetCoreInstrumentation()
          .AddRuntimeInstrumentation()
          .AddConsoleExporter());
# endregion

# region Middlewares

# region GlobalException
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
# endregion

# endregion

#region Swagger Configuration

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Analise de Risco - Desafio",
        Description = @"Classificacao automatica de operações financeiras (trades) 
                        de acordo com o nível de risco. 
                        Cada trade possui um valor monetário e o 
                        setor do cliente (Público ou Privado)",
        Contact = new OpenApiContact
        {
            Name = "Osmar Junior",
            Email = "osmarfsjunior@outlook.com"
        },
    });
});
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


# region Controllers
app.UseRouting();
app.MapControllers();

# endregion

app.Run();
