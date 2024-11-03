var builder = WebApplication.CreateBuilder(args);
//Add the Services to the Container

var app = builder.Build();
//Configure the HTTPS request pipeline


app.Run();
