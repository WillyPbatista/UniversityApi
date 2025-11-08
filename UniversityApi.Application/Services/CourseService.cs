using AutoMapper;
using UniversityApi.Domain;

public class CourseService : ICourseService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Course> _Repository;

    public CourseService(IMapper mapper, IGenericRepository<Course> repository)
    {
        _mapper = mapper;
        _Repository = repository;
    }
    public async Task<CourseDTO> CreateCourse(CourseDTO courseDTO)
    {
        var newCourse = _mapper.Map<Course>(courseDTO);

        await _Repository.AddAsync(newCourse);
        await _Repository.SaveChangesAsync();

        return courseDTO;
    }

    public async Task<bool> DeleteCourse(int Id)
    {
        var deleteCourse = await _Repository.GetByIdAsync(Id);

        if (deleteCourse == null)
        {
            _Repository.Delete(deleteCourse);
            await _Repository.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<CourseDTO> GetCourse(int Id)
    {
        var Course = await _Repository.GetByIdAsync(Id);
        return Course != null ? _mapper.Map<CourseDTO>(Course) : null;
    }

    public async Task<IEnumerable<CourseDTO>> GetCourses()
    {
        var list = await _Repository.GetAllAsync();
        return _mapper.Map<IEnumerable<CourseDTO>>(list);
    }

    public async Task<CourseDTO> UpdateCourse(CourseDTO courseDTO)
    {
        var UpdateCourse = await _Repository.GetByIdAsync(courseDTO.id);
        if (UpdateCourse != null)

        {
            _Repository.Update(UpdateCourse);
            await _Repository.SaveChangesAsync();

            return _mapper.Map<CourseDTO>(UpdateCourse);
        }
        return null;
    }
}