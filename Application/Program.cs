
using API.Interfaces;
using API.Security;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using Domain.Interfaces;
using Domain.Entities;
using Infrastructure.Repositories;
using Services.Abstractions.Interfaces;
using Services.Services;
using Service.Services;
using Infrastructure;
using Services.Abstractions.DTO;
using Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
builder.Services.AddScoped<IJWTManager,JWTManager>();
builder.Services.AddScoped<IVacanciesService,VacanciesService>();
builder.Services.AddScoped<INotificationsService,NotificationsService>();
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<IVacancyRepository,VacancyRepository>();
builder.Services.AddScoped<ITaskValidationService,TaskValidationService>();
builder.Services.AddScoped<IVikorService,VikorService>();
builder.Services.AddScoped<ICriteriasRepository,CriteriasRepository>();
builder.Services.AddScoped<IAnalysisService,AnalysisService>();
builder.Services.AddScoped<IAnalysisRepository,AnalysisRepository>();
builder.Services.AddScoped<ICriteriasService,CriteriasService>();
builder.Services.AddScoped<ICandidatesService,CandidatesService>();
builder.Services.AddScoped<ICandidatesRepository,CandidatesRepository>();
builder.Services.AddScoped<IVikorService,VikorService>();
builder.Services.AddScoped<IPresentationService,PresentationService>();
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("https://localhost:5292", "http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        })
    );



builder.Services.AddDbContext<ApplicationContext>(options => 
        options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
            options.SignIn.RequireConfirmedAccount = false;
        }).AddEntityFrameworkStores<ApplicationContext>();




builder.Services.AddAuthentication(options =>
{
     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
     options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanSeeUsers", policy =>
        policy.RequireAssertion(context => 
        context.User.IsInRole("Admin")));
    options.AddPolicy("CanAddUsers", policy =>
        policy.RequireAssertion(context => 
        context.User.IsInRole("Admin")));
});


var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }