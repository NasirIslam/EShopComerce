var builder = WebApplication.CreateBuilder(args);
//Add the Services to the Container
builder.Services.AddCarter();
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof (Program).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof (Program).Assembly);
builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("database")!);
}
).UseLightweightSessions();

var app = builder.Build();
//Configure the HTTPS request pipeline
app.MapCarter();


app.Run();
