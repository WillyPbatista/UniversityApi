public interface IEnrollmentService
{
    Task<IEnumerable<EnrollmentDTO>> GetEnrollments();
    Task<EnrollmentDTO> GetEnrollment(int Id);
    Task<bool> DeleteEnrollment(int Id);
}