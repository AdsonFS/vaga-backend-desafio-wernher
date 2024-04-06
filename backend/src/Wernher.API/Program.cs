using Wernher.API;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureServices(builder.Configuration);
builder.AddSerilogApi(builder.Configuration);

var app = builder.Build();
app.ConfigureApp();

app.Run();
