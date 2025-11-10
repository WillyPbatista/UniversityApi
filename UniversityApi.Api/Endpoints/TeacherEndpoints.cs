using Microsoft.AspNetCore.Mvc;
using UniversityApi.Application;

namespace UniversityApi.Api
{
    public static class TeacherEndpoints
    {
        public static void MapTeacherEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/teachers")
            .WithTags("Teachers");

            group.MapGet("/", async ([FromServices] ITeacherService service) =>
            {
                var list = await service.GetTeachers();
                return Results.Ok(list);
            })
            .WithName("GetAllTeachers");

            group.MapGet("/{id:int}", async (int id, [FromServices] ITeacherService service) =>
            {
                var Teacher = await service.GetTeacher(id);
                return Teacher is null ? Results.NotFound() : Results.Ok(Teacher);
            })
            .WithName("GetTeacher");

            group.MapPost("/", async ([FromBody] TeacherCreateDTO TeacherCreateDto, [FromServices] ITeacherService service) =>
            {
                if (TeacherCreateDto is null) return Results.BadRequest();
                var Teacher = await service.CreateTeacher(TeacherCreateDto);
                return Results.Created($"/api/Teachers/{Teacher.Name}", Teacher);
            })
            .WithName("CreateTeacher");
        }
    }
}