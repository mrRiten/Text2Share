using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TextMicroService.Application.Repositories;
using TextMicroService.Application.Services;
using TextMicroService.Core;
using TextMicroService.Core.Models;
using TextMicroService.Infrastructure.Repositories;
using TextMicroService.Infrastructure.Services;
using Newtonsoft.Json.Serialization;
using TextMicroService.Core.Attributes;
using TextMicroService.Application.Helpers;
using TextMicroService.Infrastructure.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<TextMicroServiceContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("TextMicroService.API")),
        ServiceLifetime.Scoped);

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<ITextRepository, TextRepository>();
builder.Services.AddScoped<ITextService, TextService>();
builder.Services.AddScoped<IHttpHelper, HttpHelper>();

builder.Services.AddScoped<ValidateSourceFilter>();

builder.Services.Configure<XSource>(builder.Configuration.GetSection("SourceToken"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ??
    throw new ArgumentNullException("Can`t find JwtSettings in appsettings.json");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerParameters =>
{
    swaggerParameters.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter JWT format: Bearer + token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    swaggerParameters.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
