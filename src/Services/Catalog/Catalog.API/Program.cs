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
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();
//Configure the HTTPS request pipeline
app.MapCarter();
app.UseExceptionHandler(options => { });

app.Run();
