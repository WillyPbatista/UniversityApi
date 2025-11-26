using Microsoft.AspNetCore.Mvc;
using UniversityApi.Application;

namespace UniversityApi.Api
{
    public static class StudentEndpoints
    {
        public static void MapStudentEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/stundents")
            .WithTags("Students")
            .RequireAuthorization();

            group.MapGet("/", async ([FromServices] IStudentService service) =>
            {
                var list = await service.GetStudents();
                return Results.Ok(list);
            })
            .WithName("GetAllStudents")
            .RequireAuthorization();

            group.MapGet("/{id:int}", async (int id, [FromServices] IStudentService service) =>
            {
                var student = await service.GetStudent(id);
                return student is null ? Results.NotFound() : Results.Ok(student);
            })
            .WithName("GetStudent")
            .RequireAuthorization();

            group.MapPost("/", async ([FromBody] StudentCreateDTO studentCreateDto, [FromServices] IStudentService service) =>
            {
                if (studentCreateDto is null) return Results.BadRequest();
                var student = await service.CreateStudent(studentCreateDto);
                return Results.Created($"/api/students/{student.Name}", student);
            })
            .WithName("CreateStudent")
            .RequireAuthorization();

            group.MapPut("/{id:int}", async (int id, [FromBody] StudentCreateDTO studentCreateDto, [FromServices] IStudentService service) =>
            {
                if (studentCreateDto is null) return Results.BadRequest();
                var student = await service.UpdateStudent(studentCreateDto, id);
                return Results.Ok(student);
            })
            .WithName("UpdateStudent")
            .RequireAuthorization();

            group.MapDelete("/{id:int}", async (int id, IStudentService service) =>
            {
                if (id == null) return Results.BadRequest();
                var student = await service.DeleteStudent( id);
                return Results.NoContent();
            })
            .WithName("DeleteStudent")
            .RequireAuthorization();
        }
    }
}