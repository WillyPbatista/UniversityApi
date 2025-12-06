using Xunit;
using Moq;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UniversityApi.Domain;

public class EnrollmentServiceTest
{
    private readonly Mock<IGenericRepository<Enrollment>> _enrollmentRepoMock;
    private readonly Mock<IGenericRepository<Student>> _studentRepoMock;
    private readonly Mock<IGenericRepository<Course>> _courseRepoMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly EnrollmentService _service;

    public EnrollmentServiceTest()
    {
        _enrollmentRepoMock = new Mock<IGenericRepository<Enrollment>>();
        _studentRepoMock = new Mock<IGenericRepository<Student>>();
        _courseRepoMock = new Mock<IGenericRepository<Course>>();
        _mapperMock = new Mock<IMapper>();

        _service = new EnrollmentService(
            _mapperMock.Object,
            _enrollmentRepoMock.Object,
            _studentRepoMock.Object,
            _courseRepoMock.Object
        );
    }

    // -------------------------------------------------------
    // CREATE
    // -------------------------------------------------------

    [Fact]
    public async Task CreateEnrollment_ShouldThrowArgumentNullException_WhenDtoIsNull()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _service.CreateEnrollment(null));
    }

    [Fact]
    public async Task CreateEnrollment_ShouldThrowArgumentException_WhenStudentIdIsInvalid()
    {
        var dto = new EnrollmentCreateDTO { StudentId = 0, CourseId = 1 };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CreateEnrollment(dto));
    }

    [Fact]
    public async Task CreateEnrollment_ShouldThrowArgumentException_WhenCourseIdIsInvalid()
    {
        var dto = new EnrollmentCreateDTO { StudentId = 1, CourseId = 0 };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CreateEnrollment(dto));
    }

    [Fact]
    public async Task CreateEnrollment_ShouldThrowKeyNotFoundException_WhenStudentDoesNotExist()
    {
        var dto = new EnrollmentCreateDTO { StudentId = 1, CourseId = 2 };

        _studentRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Student)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.CreateEnrollment(dto));
    }

    [Fact]
    public async Task CreateEnrollment_ShouldThrowKeyNotFoundException_WhenCourseDoesNotExist()
    {
        var dto = new EnrollmentCreateDTO { StudentId = 1, CourseId = 2 };

        _studentRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Student());
        _courseRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Course)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.CreateEnrollment(dto));
    }

    [Fact]
    public async Task CreateEnrollment_ShouldThrowInvalidOperationException_WhenEnrollmentAlreadyExists()
    {
        var dto = new EnrollmentCreateDTO { StudentId = 1, CourseId = 2 };

        _studentRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Student());
        _courseRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(new Course());

        _enrollmentRepoMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Enrollment> {
                new Enrollment { StudentId = 1, CourseId = 2 }
            });

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateEnrollment(dto));
    }

    [Fact]
    public async Task CreateEnrollment_ShouldCreateEnrollment_WhenDataIsValid()
    {
        var dto = new EnrollmentCreateDTO { StudentId = 1, CourseId = 2 };
        var entity = new Enrollment { StudentId = 1, CourseId = 2 };

        _studentRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Student());
        _courseRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(new Course());
        _enrollmentRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Enrollment>());

        _mapperMock.Setup(m => m.Map<Enrollment>(dto)).Returns(entity);

        _enrollmentRepoMock.Setup(r => r.AddAsync(entity)).Returns(Task.CompletedTask);
        _enrollmentRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var result = await _service.CreateEnrollment(dto);

        Assert.Equal(dto.StudentId, result.StudentId);
        Assert.Equal(dto.CourseId, result.CourseId);
    }

    // -------------------------------------------------------
    // DELETE
    // -------------------------------------------------------

    [Fact]
    public async Task DeleteEnrollment_ShouldThrowKeyNotFoundException_WhenEnrollmentDoesNotExist()
    {
        _enrollmentRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((Enrollment)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.DeleteEnrollment(1));
    }

    [Fact]
    public async Task DeleteEnrollment_ShouldDeleteEnrollment_WhenEnrollmentExists()
    {
        var enrollment = new Enrollment { Id = 1 };

        _enrollmentRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(enrollment);
        _enrollmentRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var result = await _service.DeleteEnrollment(1);

        Assert.True(result);

        _enrollmentRepoMock.Verify(r => r.Delete(enrollment), Times.Once);
        _enrollmentRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    // -------------------------------------------------------
    // GET BY ID
    // -------------------------------------------------------

    [Fact]
    public async Task GetEnrollment_ShouldThrowKeyNotFoundException_WhenEnrollmentDoesNotExist()
    {
        _enrollmentRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((Enrollment)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.GetEnrollment(1));
    }

    [Fact]
    public async Task GetEnrollment_ShouldReturnEnrollmentDto_WhenEnrollmentExists()
    {
        var enrollment = new Enrollment { Id = 1 };
        var dto = new EnrollmentDTO { Id = 1 };

        _enrollmentRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(enrollment);
        _mapperMock.Setup(m => m.Map<EnrollmentDTO>(enrollment)).Returns(dto);

        var result = await _service.GetEnrollment(1);

        Assert.Equal(1, result.Id);
    }

    // -------------------------------------------------------
    // GET ALL
    // -------------------------------------------------------

    [Fact]
    public async Task GetEnrollments_ShouldReturnEmptyList_WhenNoEnrollmentsExist()
    {
        _enrollmentRepoMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Enrollment>());

        _mapperMock.Setup(m => m.Map<IEnumerable<EnrollmentDTO>>(It.IsAny<IEnumerable<Enrollment>>()))
            .Returns(new List<EnrollmentDTO>());

        var result = await _service.GetEnrollments();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetEnrollments_ShouldReturnListOfEnrollmentDtos_WhenEnrollmentsExist()
    {
        var list = new List<Enrollment> { new Enrollment { Id = 1 } };
        var dtoList = new List<EnrollmentDTO> { new EnrollmentDTO { Id = 1 } };

        _enrollmentRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

        _mapperMock.Setup(m =>
            m.Map<IEnumerable<EnrollmentDTO>>(list))
            .Returns(dtoList);

        var result = await _service.GetEnrollments();

        Assert.Single(result);
        Assert.Equal(1, result.First().Id);
    }
}
