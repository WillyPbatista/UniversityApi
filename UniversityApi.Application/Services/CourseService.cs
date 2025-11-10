using AutoMapper;
using UniversityApi.Domain;

public class CourseService : ICourseService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Course> _Repository;
    private readonly ICourseRepository _courseRepository;
    public CourseService(IMapper mapper, IGenericRepository<Course> repository, ICourseRepository courseRepository)
    {
        _mapper = mapper;
        _Repository = repository;
        _courseRepository = courseRepository;
    }
    public async Task<CourseCreateDTO> CreateCourse(CourseCreateDTO courseCreateDTO)
    {
        var newCourse = _mapper.Map<Course>(courseCreateDTO);

        await _Repository.AddAsync(newCourse);
        await _Repository.SaveChangesAsync();

        return courseCreateDTO;
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
        var Course = await _courseRepository.GetById(Id);
        return Course != null ? _mapper.Map<CourseDTO>(Course) : null;
    }

    public async Task<IEnumerable<CourseDTO>> GetCourses()
    {
        var list = await _courseRepository.GetAll();
        return _mapper.Map<IEnumerable<CourseDTO>>(list);
    }

    public async Task<CourseDTO> UpdateCourse(CourseDTO courseDTO)
    {
        var UpdateCourse = await _Repository.GetByIdAsync(courseDTO.Id);
        if (UpdateCourse != null)

        {
            _Repository.Update(UpdateCourse);
            await _Repository.SaveChangesAsync();

            return _mapper.Map<CourseDTO>(UpdateCourse);
        }
        return null;
    }
}