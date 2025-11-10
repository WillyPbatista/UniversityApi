using AutoMapper;
using UniversityApi.Domain;
public class TeacherService : ITeacherService
{
    private readonly IGenericRepository<Teacher> _Repository;
    private readonly IMapper _Mapper;


    public TeacherService(IGenericRepository<Teacher> repository, IMapper mapper)
    {
        _Repository = repository;
        _Mapper = mapper;
    }
    public async Task<TeacherCreateDTO> CreateTeacher(TeacherCreateDTO Teacher)
    {
        var newTeacher = _Mapper.Map<Teacher>(Teacher);
        await _Repository.AddAsync(newTeacher);
        await _Repository.SaveChangesAsync();
        return Teacher;
    }

    public async Task<bool> DeleteTeacher(int id)
    {
        var deleteTeacher = await _Repository.GetByIdAsync(id);

        if (deleteTeacher != null)
        {
            _Repository.Delete(deleteTeacher);
            await _Repository.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<TeacherDTO> GetTeacher(int id)
    {
        var Teacher = await _Repository.GetByIdAsync(id);

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

        if (UpdatedTeacher != null)
        {
            _Repository.Update(UpdatedTeacher);
            await _Repository.SaveChangesAsync();
            return _Mapper.Map<TeacherDTO>(UpdatedTeacher);
        }
        return null;
    }
}