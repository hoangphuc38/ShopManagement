using Microsoft.EntityFrameworkCore;
using ShopManagement_Backend.Middlewares;
using ShopManagement_Backend.Models;
using ShopManagement_Backend.Service;
using ShopManagement_Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ShopManagementDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShopManagement"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ShopService>();
builder.Services.AddScoped<ProductService>();   
builder.Services.AddScoped<ShopDetailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
