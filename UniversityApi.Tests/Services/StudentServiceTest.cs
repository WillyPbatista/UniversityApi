using Moq;
using Xunit;
using AutoMapper;
using UniversityApi.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

public class StudentServiceTests
{
    private readonly Mock<IStudentRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly StudentService _service;

    public StudentServiceTests()
    {
        _mockRepo = new Mock<IStudentRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new StudentService(_mockRepo.Object, _mockMapper.Object);
    }

    // -------------------------------------------------
    // TEST: CREATE STUDENT
    // -------------------------------------------------
    [Fact]
    public async Task CreateStudent_ShouldCreate_WhenValid()
    {
        var createDto = new StudentCreateDTO
        {
            Name = "William",
            Email = "test@test.com",
            EnrollmentDate = DateTime.Now
        };

        var entity = new Student
        {
            Id = 1,
            Name = createDto.Name,
            Email = createDto.Email,
            EnrollmentDate = createDto.EnrollmentDate
        };

        _mockRepo.Setup(r => r.GetAllAsync())
                 .ReturnsAsync(new List<Student>());

        _mockMapper.Setup(m => m.Map<Student>(createDto))
                   .Returns(entity);

        _mockRepo.Setup(r => r.AddAsync(entity))
                 .Returns(Task.CompletedTask);

        _mockRepo.Setup(r => r.SaveChangesAsync())
                 .Returns(Task.CompletedTask);

        var result = await _service.CreateStudent(createDto);

        Assert.Equal("William", result.Name);
        Assert.Equal("test@test.com", result.Email);
    }

    // -------------------------------------------------
    // TEST: GET STUDENT
    // -------------------------------------------------
    [Fact]
    public async Task GetStudent_ShouldReturnDto_WhenExists()
    {
        var entity = new Student
        {
            Id = 1,
            Name = "Willy",
            Email = "willy@test.com"
        };

        var dto = new StudentDTO
        {
            Id = 1,
            Name = "Willy",
            Email = "willy@test.com"
        };

        _mockRepo.Setup(r => r.GetByIdAsync(1))
                 .ReturnsAsync(entity);

        _mockMapper.Setup(m => m.Map<StudentDTO>(entity))
                   .Returns(dto);

        var result = await _service.GetStudent(1);

        Assert.Equal(1, result.Id);
        Assert.Equal("Willy", result.Name);
    }

    // -------------------------------------------------
    // TEST: GET STUDENTS (LIST)
    // -------------------------------------------------
    [Fact]
    public async Task GetStudents_ShouldReturnListOfDtos()
    {
        var list = new List<Student>
        {
            new Student { Id = 1, Name = "Uno" },
            new Student { Id = 2, Name = "Dos" }
        };

        var dtoList = new List<StudentDTO>
        {
            new StudentDTO { Id = 1, Name = "Uno" },
            new StudentDTO { Id = 2, Name = "Dos" }
        };

        _mockRepo.Setup(r => r.GetAllAsync())
                 .ReturnsAsync(list);

        _mockMapper.Setup(m => m.Map<IEnumerable<StudentDTO>>(list))
                   .Returns(dtoList);

        var result = (await _service.GetStudents()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("Uno", result[0].Name);
    }

    // -------------------------------------------------
    // TEST: DELETE STUDENT
    // -------------------------------------------------
    [Fact]
    public async Task DeleteStudent_ShouldReturnTrue_WhenExists()
    {
        var entity = new Student { Id = 1, Name = "Willy" };

        _mockRepo.Setup(r => r.GetByIdAsync(1))
                 .ReturnsAsync(entity);

        _mockRepo.Setup(r => r.Delete(entity));

        _mockRepo.Setup(r => r.SaveChangesAsync())
                 .Returns(Task.CompletedTask);

        var result = await _service.DeleteStudent(1);

        Assert.True(result);
    }

    // -------------------------------------------------
    // TEST: UPDATE STUDENT
    // -------------------------------------------------
    [Fact]
    public async Task UpdateStudent_ShouldUpdate_WhenValid()
    {
        var updateDto = new StudentCreateDTO
        {
            Name = "Nuevo nombre",
            Email = "nuevo@test.com",
            EnrollmentDate = DateTime.Now
        };

        var entity = new Student
        {
            Id = 1,
            Name = "Viejo",
            Email = "viejo@test.com",
            EnrollmentDate = DateTime.Now.AddYears(-1)
        };

        _mockRepo.Setup(r => r.GetByIdAsync(1))
                 .ReturnsAsync(entity);

        _mockRepo.Setup(r => r.SaveChangesAsync())
                 .Returns(Task.CompletedTask);

        var result = await _service.UpdateStudent(updateDto, 1);

        Assert.Equal("Nuevo nombre", result.Name);
        Assert.Equal("nuevo@test.com", result.Email);
    }
}
