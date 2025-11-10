using AutoMapper;
using UniversityApi.Domain;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<StudentDTO, Student>().ReverseMap();
        CreateMap<StudentCreateDTO, Student>().ReverseMap();
        CreateMap<CourseDTO, Course>().ReverseMap();
        CreateMap<CourseCreateDTO, Course>().ReverseMap();
        CreateMap<EnrollmentDTO, Enrollment>().ReverseMap();
        CreateMap<EnrollmentCreateDTO, Enrollment>().ReverseMap();
        CreateMap<TeacherCreateDTO, Teacher>().ReverseMap();
        CreateMap<TeacherDTO, Teacher>().ReverseMap();
    }
}