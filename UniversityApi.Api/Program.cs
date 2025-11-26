using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using UniversityApi.Api;
using UniversityApi.Api.Middlewares;
using UniversityApi.Application;
using UniversityApi.Identity.Extensions;
using UniversityApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = builder.Configuration.GetConnectionString("UniversityConnection");
builder.Services.AddDbContext<UniversityApiDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddIdentityModule(builder.Configuration);



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT en este formato: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    
app.UseAuthentication();
app.UseAuthorization();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseCors("AllowAll"); 
app.MapStudentEndpoints();
app.MapCourseEnpoints();
app.MapTeacherEndpoints();
app.MapEnrollmentEnpoints();
app.MapAuthEnpoints();


app.UseCors("AllowAll");

app.UseHttpsRedirection();


app.Run();

