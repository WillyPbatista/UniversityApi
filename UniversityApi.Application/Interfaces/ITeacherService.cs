using UniversityApi.Domain;

public interface ITeacherService
{
    Task<IEnumerable<TeacherDTO>> GetTeachers();
    Task<TeacherDTO> GetTeacher(int id);
    Task<TeacherCreateDTO> CreateTeacher(TeacherCreateDTO Teacher);
    Task<TeacherCreateDTO> UpdateTeacher(TeacherCreateDTO Teacher, int id);
    Task<bool> DeleteTeacher(int id);

}