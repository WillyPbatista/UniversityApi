using AutoMapper;
using UniversityApi.Domain;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<StudentDTO, Student>().ReverseMap();
        CreateMap<StudentCreateDTO, Student>().ReverseMap();
        CreateMap<CourseDTO, Course>().ReverseMap();
        CreateMap<EnrollmentDTO, Enrollment>().ReverseMap();
    }
}