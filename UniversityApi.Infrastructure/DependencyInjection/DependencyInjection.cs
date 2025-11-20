using Microsoft.Extensions.DependencyInjection;
using UniversityApi.Domain;
using UniversityApi.Infrastructure;

namespace UniversityApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(ICourseRepository), typeof(CourseRepository));
            services.AddScoped(typeof(IStudentRepository), typeof(StudentRepository));
            services.AddScoped(typeof(ITeacherRepository), typeof(TeacherRepository));

            return services;
        }
    }
}