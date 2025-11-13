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
        if (courseCreateDTO == null)
            throw new ArgumentNullException(nameof(courseCreateDTO), "The request body cannot be empty");

        if (string.IsNullOrWhiteSpace(courseCreateDTO.Title))
            throw new ArgumentException("Course title is required.", nameof(courseCreateDTO.Title));

        if (string.IsNullOrWhiteSpace(courseCreateDTO.Credits) || courseCreateDTO.Credits == null)
            throw new ArgumentException("Course credits is required", nameof(courseCreateDTO.Credits));

        if (courseCreateDTO.TeacherId == null)
            throw new ArgumentNullException(nameof(CourseCreateDTO.TeacherId), "Teacher ID is required.");

        var allcourses = await _Repository.GetAllAsync();
        if (allcourses.Any(c => c.Title.Equals(courseCreateDTO.Title, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("A course with the same title already exists.", nameof(courseCreateDTO.Title));

    
        var newCourse = _mapper.Map<Course>(courseCreateDTO);

        await _Repository.AddAsync(newCourse);
        await _Repository.SaveChangesAsync();

        return courseCreateDTO;
    }

    public async Task<bool> DeleteCourse(int Id)
    {
        var deleteCourse = await _Repository.GetByIdAsync(Id);

        if(deleteCourse == null)
            throw new ArgumentNullException("Course not found.", nameof(deleteCourse));

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

        if (Course == null)
            throw new ArgumentNullException("Course not found.", nameof(Course));

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
        if (UpdateCourse == null)
            throw new ArgumentNullException("Course not found.", nameof(UpdateCourse));

        _Repository.Update(UpdateCourse);
        await _Repository.SaveChangesAsync();
        return courseDTO;

    }
}