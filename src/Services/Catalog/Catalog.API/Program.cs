using BuildingBlocks.Behaviours;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);
//Add the Services to the Container
builder.Services.AddCarter();
builder.Services.AddMediatR(
    config =>
    {
        config.RegisterServicesFromAssembly(typeof(Program).Assembly);
        config.AddOpenBehavior(typeof(ValidationBehaviours<,>));
    }
    );
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("database")!);
}
).UseLightweightSessions();

var app = builder.Build();
//Configure the HTTPS request pipeline
app.MapCarter();
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (exception == null)
            return;
        var problemDetails = new ProblemDetails
        {
            Title = exception.Message,
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.StackTrace
        };
        var logger = context.RequestServices.GetService<ILogger<Program>>();
        logger.LogError(exception, exception.Message);
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problemDetails);


    });
});



app.Run();
