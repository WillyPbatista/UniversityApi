using UniversityApi.Identity.DTOs;
using UniversityApi.Identity.Services;

namespace UniversityApi.Api
{
    public static class AuthEndpoints
    {
        public static void MapAuthEnpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/auth")
            .WithTags("RBAC");

            app.MapPost("/api/auth/register-student", async (RegisterStudentDto dto, IAuthService auth) =>
        {
            var result = await auth.RegisterStudentAsync(dto);
            return result.Succeeded ? Results.Ok() : Results.BadRequest(result.Errors);
        }).WithName("RegisterStudent");

            app.MapPost("/api/auth/register-teacher", async (RegisterTeacherDto dto, IAuthService auth) =>
        {
            var result = await auth.RegisterTeacherAsync(dto);
            return result.Succeeded ? Results.Ok() : Results.BadRequest(result.Errors);
        }).WithName("RegisterTeacher");

            app.MapPost("/api/auth/login", async (LoginDto dto, IAuthService auth) =>
            {
                var token = await auth.LoginAsync(dto);
                return token is null ? Results.Unauthorized() : Results.Ok(new { token });
            }).WithName("Login");

        }
    }
}