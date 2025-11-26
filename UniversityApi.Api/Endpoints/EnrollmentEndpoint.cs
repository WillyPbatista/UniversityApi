using Microsoft.AspNetCore.Mvc;
using UniversityApi.Application;

namespace UniversityApi.Api
{
    public static class EnrollmentEndpoints
    {
        public static void MapEnrollmentEnpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Enrollements")
            .WithTags("Enrollments")
            .RequireAuthorization();

            group.MapGet("/", async ([FromServices] IEnrollmentService service) =>
            {
                var list = await service.GetEnrollments();
                return Results.Ok(list);
            })
            .WithName("GetEnrollments")
            .RequireAuthorization();
            
            group.MapPost("/", async ([FromBody] EnrollmentCreateDTO enrollmentCreateDTO, IEnrollmentService service ) =>
            {
                if (enrollmentCreateDTO is null) return Results.BadRequest();
                var enrollment = await service.CreateEnrollment(enrollmentCreateDTO);
                return Results.Created();
            })
            .WithName("CreateEnrollment")
            .RequireAuthorization();
        }
    }
}