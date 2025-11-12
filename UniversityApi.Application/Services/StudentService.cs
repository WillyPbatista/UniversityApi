using System.ComponentModel.DataAnnotations;
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
    public async Task<StudentCreateDTO> CreateStudent(StudentCreateDTO student)
    {
        if (student == null)
            throw new ArgumentNullException(nameof(student), "The request body cannot be null.");

        if (string.IsNullOrWhiteSpace(student.Name))
            throw new ArgumentException("Student name is a required field", nameof(student.Name));

        if (string.IsNullOrWhiteSpace(student.Email))
            throw new ArgumentException("Student email is a required field", nameof(student.Email));

        var allStudents = await _Repository.GetAllAsync();
        if (allStudents.Any(s => s.Email.Equals(student.Email, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("A student with that email already exists.");

        if (!new EmailAddressAttribute().IsValid(student.Email))
            throw new ArgumentException("invalid email format", nameof(student.Email));


        var newStudent = _Mapper.Map<Student>(student);
        await _Repository.AddAsync(newStudent);
        await _Repository.SaveChangesAsync();
        return student;
    }

    public async Task<bool> DeleteStudent(int id)
    {
        var deleteStudent = await _Repository.GetByIdAsync(id);

        if (deleteStudent == null)
        {
            throw new KeyNotFoundException($"No student found with id {id}");
        }
        _Repository.Delete(deleteStudent);
        await _Repository.SaveChangesAsync();

        return true;
    }

    public async Task<StudentDTO> GetStudent(int id)
    {
        var student = await _Repository.GetByIdAsync(id);

        if (student == null)
            throw new KeyNotFoundException($"No student found with ID {id}");


        return student == null ? null : _Mapper.Map<StudentDTO>(student);
    }

    public async Task<IEnumerable<StudentDTO>> GetStudents()
    {
        var list = await _Repository.GetAllAsync();
        return _Mapper.Map<IEnumerable<StudentDTO>>(list);
    }

    public async Task<StudentDTO> UpdateStudent(StudentDTO student, int id)
    {

        if (student == null)
            throw new ArgumentNullException(nameof(student), "the request body cannot be null.");

        var existingStudent = await _Repository.GetByIdAsync(id);
        if (existingStudent == null)
            throw new KeyNotFoundException($"No student found with ID {id}");

        if (string.IsNullOrWhiteSpace(student.Name))
            throw new ArgumentException("Student name is required", nameof(student));

        if (string.IsNullOrWhiteSpace(student.Email))
            throw new ArgumentException("Student email is required", nameof(student));

        if (!new EmailAddressAttribute().IsValid(student.Email))
            throw new ArgumentException("invalid email format", nameof(student.Email));

        var UpdateStudent = _Mapper.Map<Student>(student);    
        _Repository.Update(UpdateStudent);
        await _Repository.SaveChangesAsync();
        return student;
    }
}