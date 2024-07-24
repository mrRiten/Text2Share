using EmailMicroService.Application.Repositories;
using EmailMicroService.Application.Services;
using EmailMicroService.Core;
using EmailMicroService.Core.Models;
using EmailMicroService.Infrastructure.Repositories;
using EmailMicroService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddDbContext<EmailMicroServiceContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("EmailMicroService.API")),
        ServiceLifetime.Scoped);

builder.Services.Configure<SMTP>(builder.Configuration.GetSection("SMTP"));

builder.Services.AddHostedService<EmailBackgroundService>();
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();

builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

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
