using Microsoft.EntityFrameworkCore;
using TextMicroService.Application.Repositories;
using TextMicroService.Application.Services;
using TextMicroService.Core;
using TextMicroService.Infrastructure.Repositories;
using TextMicroService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<TextMicroServiceContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("TextMicroService.API")),
        ServiceLifetime.Scoped);

builder.Services.AddScoped<ITextRepository, TextRepository>();
builder.Services.AddScoped<ITextService, TextService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
