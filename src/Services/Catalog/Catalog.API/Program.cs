var builder = WebApplication.CreateBuilder(args);
//Add the Services to the Container
builder.Services.AddCarter();
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof (Program).Assembly));

var app = builder.Build();
//Configure the HTTPS request pipeline
app.MapCarter();


app.Run();
