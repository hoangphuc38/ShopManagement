using ShopManagement_Backend_API;
using ShopManagement_Backend_API.Middlewares;
using ShopManagement_Backend_Application;
using ShopManagement_Backend_Application.Hubs;
using ShopManagement_Backend_DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwagger();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

builder.Services.AddDataAccess(builder.Configuration)
                .AddApplication();

builder.Services.AddJwt(builder.Configuration);

builder.Services.AddMemoryCache();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseMiddleware<AuthorizationMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAll");

app.MapHub<StockHub>("/hubs/stock");

app.Run();

namespace ShopManagement_Backend_API
{
    public partial class Program { }
}
