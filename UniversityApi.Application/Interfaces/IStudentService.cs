public interface IStudentService
{
    Task<IEnumerable<StudentDTO>> GetStudents();
    Task<StudentDTO> GetStudent(int id);
    Task<StudentCreateDTO> CreateStudent(StudentCreateDTO student);
    Task<StudentDTO> UpdateStudent(StudentDTO student, int id);
    Task<bool> DeleteStudent(int id);

}