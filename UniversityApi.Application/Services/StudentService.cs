using AutoMapper;
using UniversityApi.Domain;
public class StudentService : IStudentService
{
    private readonly IGenericRepository<Student> _Repository;
    private readonly IMapper _Mapper;


    public StudentService(IGenericRepository<Student> repository, IMapper mapper)
    {
        _Repository = repository;
        _Mapper = mapper;
    }
    public async Task<StudentDTO> CreateStudent(StudentDTO student)
    {
        var newStudent = _Mapper.Map<Student>(student);
        await _Repository.AddAsync(newStudent);
        await _Repository.SaveChangesAsync();
        return student;
    }

    public async Task<bool> DeleteStudent(int id)
    {
        var deleteStudent = await _Repository.GetByIdAsync(id);

        if (deleteStudent != null)
        {
            _Repository.Delete(deleteStudent);
            await _Repository.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<StudentDTO> GetStudent(int id)
    {
        var student = await _Repository.GetByIdAsync(id);

        return student == null ? null : _Mapper.Map<StudentDTO>(student);
    }

    public async Task<IEnumerable<StudentDTO>> GetStudents()
    {
        var list = await _Repository.GetAllAsync();
        return _Mapper.Map<IEnumerable<StudentDTO>>(list);
    }

    public async Task<StudentDTO> UpdateStudent(StudentDTO student, int id)
    {
        var UpdatedStudent = await _Repository.GetByIdAsync(id);

        if (UpdatedStudent != null)
        {
            _Repository.Update(UpdatedStudent);
            await _Repository.SaveChangesAsync();
            return _Mapper.Map<StudentDTO>(UpdatedStudent);
        }
        return null;
    }
}