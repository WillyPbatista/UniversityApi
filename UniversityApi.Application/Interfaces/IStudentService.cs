public interface IStudentService
{
    Task<IEnumerable<StudentDTO>> GetStudents();
    Task<StudentDTO> GetStudent(int id);
    Task<StudentDTO> CreateStudent(StudentDTO student);
    Task<StudentDTO> UpdateStudent(StudentDTO student, int id);
    Task<bool> DeleteStudent(int id);

}