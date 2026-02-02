using Microsoft.OpenApi;
using RiskAnalisys.API.Middleware;
using RiskAnalisys.Application.DI;
using RiskAnalisys.Application.Serialization;
using System.Text.Json.Serialization;

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

# region

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
