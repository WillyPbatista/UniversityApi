using AutoMapper;
using UniversityApi.Domain;

public class EnrollmentService : IEnrollmentService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Enrollment> _repository;
    private readonly IGenericRepository<Student> _studentRepository;
    private readonly IGenericRepository<Course> _courseRepository;

    public EnrollmentService(
        IMapper mapper,
        IGenericRepository<Enrollment> repository,
        IGenericRepository<Student> studentRepository,
        IGenericRepository<Course> courseRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    public async Task<EnrollmentCreateDTO> CreateEnrollment(EnrollmentCreateDTO dto)
    {

        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "Enrollment data is required.");

        if (dto.StudentId <= 0)
            throw new ArgumentException("Student ID is required.");

        if (dto.CourseId <= 0)
            throw new ArgumentException("Course ID is required.");

        var student = await _studentRepository.GetByIdAsync(dto.StudentId);
        if (student == null)
            throw new KeyNotFoundException("The specified student does not exist.");

        var course = await _courseRepository.GetByIdAsync(dto.CourseId);
        if (course == null)
            throw new KeyNotFoundException("The specified course does not exist.");

        var existingEnrollments = await _repository.GetAllAsync();
        if (existingEnrollments.Any(e =>
                e.StudentId == dto.StudentId &&
                e.CourseId == dto.CourseId))
        {
            throw new InvalidOperationException("This student is already enrolled in the specified course.");
        }

        var newEnrollment = _mapper.Map<Enrollment>(dto);
        await _repository.AddAsync(newEnrollment);
        await _repository.SaveChangesAsync();

        return dto;
    }

    public async Task<bool> DeleteEnrollment(int id)
    {
        var enrollment = await _repository.GetByIdAsync(id);

        if (enrollment == null)
            throw new KeyNotFoundException("Enrollment not found.");

        _repository.Delete(enrollment);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<EnrollmentDTO> GetEnrollment(int id)
    {
        var enrollment = await _repository.GetByIdAsync(id);

        if (enrollment == null)
            throw new KeyNotFoundException("Enrollment not found.");

        return _mapper.Map<EnrollmentDTO>(enrollment);
    }

    public async Task<IEnumerable<EnrollmentDTO>> GetEnrollments()
    {
        var list = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<EnrollmentDTO>>(list);
    }
}
