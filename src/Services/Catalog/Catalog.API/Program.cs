using Catalog.API.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
//Add the Services to the Container
builder.Services.AddCarter();
builder.Services.AddMediatR(
    config =>
    {
        config.RegisterServicesFromAssembly(typeof(Program).Assembly);
        config.AddOpenBehavior(typeof(ValidationBehaviours<,>));
        config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    }
    );
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("database")!);
}
).UseLightweightSessions();
if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("database")!);

var app = builder.Build();
//Configure the HTTPS request pipeline
app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/healthChecks" , new HealthCheckOptions { ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse});

app.Run();
