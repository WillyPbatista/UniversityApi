using Microsoft.AspNetCore.Mvc;
using UniversityApi.Application;

namespace UniversityApi.Api
{
    public static class CourseEndpoints
    {
        public static void MapCourseEnpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/courses")
            .WithTags("Courses");

            group.MapGet("/", async ([FromServices] ICourseService service) =>
            {
                var courses = await service.GetCourses();
                return Results.Ok(courses);
            })
            .WithName("GetAllCourses");

            group.MapGet("/{id:int}", async (int id, [FromServices] ICourseService service) =>
            {
                var course = await service.GetCourse(id);

                if (course is null) return Results.NotFound();
                return Results.Ok(course);
            })
            .WithName("GetCourse");

            group.MapPost("/", async ([FromBody] CourseCreateDTO courseCreateDTO, ICourseService service) =>
            {
                if (courseCreateDTO is null) return Results.BadRequest();
                await service.CreateCourse(courseCreateDTO);
                return Results.Ok(courseCreateDTO);
            })
            .WithName("CerateCourse");
        }
    }
}