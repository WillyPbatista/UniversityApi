using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UniversityApi.Identity.Data;
using UniversityApi.Identity.Entities;
using UniversityApi.Identity.Jwt;
using UniversityApi.Identity.Services;

namespace UniversityApi.Identity.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<IdentityDbContext>(opt =>
                opt.UseSqlServer(config.GetConnectionString("IdentityConnection")));


            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

            
            services.Configure<JwtSettings>(config.GetSection("JwtSettings"));
            var jwtSettings = new JwtSettings();
            config.GetSection("JwtSettings").Bind(jwtSettings);
            var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; 
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true
                };
            });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
                opt.AddPolicy("TeacherOrAdmin", p => p.RequireRole("Teacher", "Admin"));
                opt.AddPolicy("StudentOnly", p => p.RequireRole("Student"));
            });

            
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<JwtTokenGenerator>();

            return services;
        }
    }
}
