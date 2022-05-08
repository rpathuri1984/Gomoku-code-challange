using Gomoku.api;
using Gomoku.Logic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IGomokuGame, GomokuGame>();

// regiser InMemory Cahce, this does not require any dependency.
// works well with single instance. But not useful when scaling is needed
builder.Services.AddMemoryCache();

// register distributed cache services. RedisCache is a good option.
// ***************************************
// update appsettings.json file with actual Redis service.
// it wont work untill we provide valied Redis service.
// ***************************************
builder.Services.AddDistributedMemoryCache();
builder.Services.AddTransient<IRedisCacheUtility, RedisCacheUtility>();
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = builder.Configuration["Redis"];
});
// ***************************************

var app = builder.Build();

// Configure the HTTP request pipeline. as this is challange, enable openAPI for release build too
if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// use gloabal exception handler,
// to prevent sensitive data exposoure and return custom message
app.UseMiddleware<Gomoku.api.Handlers.ErrorHandlerMiddleware>();

app.Run();
