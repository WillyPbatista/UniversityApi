using Microsoft.AspNetCore.Mvc;
using UniversityApi.Application;

namespace UniversityApi.Api
{
    public static class StudentEndpoints
    {
        public static void MapStudentEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/stundents")
            .WithTags("Students");

            group.MapGet("/", async ([FromServices] IStudentService service) =>
            {
                var list = await service.GetStudents();
                return Results.Ok(list);
            })
            .WithName("GetAllStudents");

            group.MapGet("/{id:int}", async (int id, [FromServices] IStudentService service) =>
            {
                var student = await service.GetStudent(id);
                return student is null ? Results.NotFound() : Results.Ok(student);
            })
            .WithName("GetStudent");

            group.MapPost("/", async ([FromBody] StudentCreateDTO studentCreateDto, [FromServices] IStudentService service) =>
            {
                if (studentCreateDto is null) return Results.BadRequest();
                var student = await service.CreateStudent(studentCreateDto);
                return Results.Created($"/api/students/{student.Name}", student);
            })
            .WithName("CreateStudent");
        }
    }
}