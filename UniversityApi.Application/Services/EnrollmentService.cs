using AutoMapper;
using UniversityApi.Domain;
public class EnrollmentService : IEnrollmentService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Enrollment> _Repository;

    public EnrollmentService(IMapper mapper, IGenericRepository<Enrollment> repository)
    {
        _mapper = mapper;
        _Repository = repository;
    }

    public async Task<EnrollmentCreateDTO> CreateEnrollment(EnrollmentCreateDTO enrollmentCreateDTO)
    {
        if (enrollmentCreateDTO == null) return null;

        var newEnrollment = _mapper.Map<Enrollment>(enrollmentCreateDTO);
        await _Repository.AddAsync(newEnrollment);
        await _Repository.SaveChangesAsync();
         Console.WriteLine("creaated enrollment in service"  + newEnrollment);
        return enrollmentCreateDTO;
    }

    public async Task<bool> DeleteEnrollment(int Id)
    {
        var Enrollment = await _Repository.GetByIdAsync(Id);

        if (Enrollment != null)
        {
            _Repository.Delete(Enrollment);
            await _Repository.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<EnrollmentDTO> GetEnrollment(int Id)
    {
        var Enrollment = await _Repository.GetByIdAsync(Id);
        return Enrollment == null ? null : _mapper.Map<EnrollmentDTO>(Enrollment);
    }

    public async Task<IEnumerable<EnrollmentDTO>> GetEnrollments()
    {
        var list = await _Repository.GetAllAsync();
        return _mapper.Map<IEnumerable<EnrollmentDTO>>(list);
    }
}
