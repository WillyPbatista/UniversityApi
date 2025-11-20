using System.ComponentModel.DataAnnotations;
using AutoMapper;
using UniversityApi.Domain;
public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _Repository;
    private readonly IMapper _Mapper;


    public TeacherService(ITeacherRepository repository, IMapper mapper)
    {
        _Repository = repository;
        _Mapper = mapper;
    }
    public async Task<TeacherCreateDTO> CreateTeacher(TeacherCreateDTO Teacher)
    {
        if (Teacher == null)
            throw new ArgumentNullException(nameof(Teacher), "The request body cannot be null.");

        if (string.IsNullOrWhiteSpace(Teacher.Name))
            throw new ArgumentException("Teacher name is a required field", nameof(Teacher.Name));

        if (string.IsNullOrWhiteSpace(Teacher.Email))
            throw new ArgumentException("Teacher email is a required field", nameof(Teacher.Email));

        var allTeachers = await _Repository.GetAllAsync();
        if (allTeachers.Any(t => t.Email.Equals(Teacher.Email, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("A Teacher with that email already exists.");

        if (!new EmailAddressAttribute().IsValid(Teacher.Email))
            throw new ArgumentException("invalid email format", nameof(Teacher.Email));



        var newTeacher = _Mapper.Map<Teacher>(Teacher);
        await _Repository.AddAsync(newTeacher);
        await _Repository.SaveChangesAsync();
        return Teacher;
    }

    public async Task<bool> DeleteTeacher(int id)
    {
        var deleteTeacher = await _Repository.GetByIdAsync(id);

        if (deleteTeacher == null)
        {
            throw new KeyNotFoundException($"No Teacher found with id {id}");
        }
        _Repository.Delete(deleteTeacher);
        await _Repository.SaveChangesAsync();

        return true;
    }

    public async Task<TeacherDTO> GetTeacher(int id)
    {
        var Teacher = await _Repository.GetByIdAsync(id);

        if (Teacher == null)
            throw new KeyNotFoundException($"No Teacher found with ID {id}");

        return Teacher == null ? null : _Mapper.Map<TeacherDTO>(Teacher);
    }

    public async Task<IEnumerable<TeacherDTO>> GetTeachers()
    {
        var list = await _Repository.GetAllAsync();
        return _Mapper.Map<IEnumerable<TeacherDTO>>(list);
    }

    public async Task<TeacherDTO> UpdateTeacher(TeacherDTO Teacher, int id)
    {
        var UpdatedTeacher = await _Repository.GetByIdAsync(id);

        if (UpdatedTeacher == null)
            throw new KeyNotFoundException($"No Teacher found with ID {id}");
        
            _Repository.Update(UpdatedTeacher);
            await _Repository.SaveChangesAsync();
            return _Mapper.Map<TeacherDTO>(UpdatedTeacher);
        
    }
}