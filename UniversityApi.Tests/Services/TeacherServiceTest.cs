using Moq;
using Xunit;
using AutoMapper;
using UniversityApi.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

public class TeacherServiceTests
{
    private readonly Mock<ITeacherRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly TeacherService _service;

    public TeacherServiceTests()
    {
        _mockRepo = new Mock<ITeacherRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new TeacherService(_mockRepo.Object, _mockMapper.Object);
    }

    // -------------------------------------------------
    // TEST: CREATE Teacher
    // -------------------------------------------------
    [Fact]
    public async Task CreateTeacher_ShouldCreate_WhenValid()
    {
        var createDto = new TeacherCreateDTO
        {
            Name = "William",
            Email = "test@test.com",
            HireDate = DateTime.Now
        };

        var entity = new Teacher
        {
            Id = 1,
            Name = createDto.Name,
            Email = createDto.Email,
            HireDate = createDto.HireDate
        };

        _mockRepo.Setup(r => r.GetAllAsync())
                 .ReturnsAsync(new List<Teacher>());

        _mockMapper.Setup(m => m.Map<Teacher>(createDto))
                   .Returns(entity);

        _mockRepo.Setup(r => r.AddAsync(entity))
                 .Returns(Task.CompletedTask);

        _mockRepo.Setup(r => r.SaveChangesAsync())
                 .Returns(Task.CompletedTask);

        var result = await _service.CreateTeacher(createDto);

        Assert.Equal("William", result.Name);
        Assert.Equal("test@test.com", result.Email);
    }

    // -------------------------------------------------
    // TEST: GET Teacher
    // -------------------------------------------------
    [Fact]
    public async Task GetTeacher_ShouldReturnDto_WhenExists()
    {
        var entity = new Teacher
        {
            Id = 1,
            Name = "Willy",
            Email = "willy@test.com"
        };

        var dto = new TeacherDTO
        {
            Id = 1,
            Name = "Willy",
            Email = "willy@test.com"
        };

        _mockRepo.Setup(r => r.GetByIdAsync(1))
                 .ReturnsAsync(entity);

        _mockMapper.Setup(m => m.Map<TeacherDTO>(entity))
                   .Returns(dto);

        var result = await _service.GetTeacher(1);

        Assert.Equal(1, result.Id);
        Assert.Equal("Willy", result.Name);
    }

    // -------------------------------------------------
    // TEST: GET TeacherS (LIST)
    // -------------------------------------------------
    [Fact]
    public async Task GetTeachers_ShouldReturnListOfDtos()
    {
        var list = new List<Teacher>
        {
            new Teacher { Id = 1, Name = "Uno" },
            new Teacher { Id = 2, Name = "Dos" }
        };

        var dtoList = new List<TeacherDTO>
        {
            new TeacherDTO { Id = 1, Name = "Uno" },
            new TeacherDTO { Id = 2, Name = "Dos" }
        };

        _mockRepo.Setup(r => r.GetAllAsync())
                 .ReturnsAsync(list);

        _mockMapper.Setup(m => m.Map<IEnumerable<TeacherDTO>>(list))
                   .Returns(dtoList);

        var result = (await _service.GetTeachers()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("Uno", result[0].Name);
    }

    // -------------------------------------------------
    // TEST: DELETE Teacher
    // -------------------------------------------------
    [Fact]
    public async Task DeleteTeacher_ShouldReturnTrue_WhenExists()
    {
        var entity = new Teacher { Id = 1, Name = "Willy" };

        _mockRepo.Setup(r => r.GetByIdAsync(1))
                 .ReturnsAsync(entity);

        _mockRepo.Setup(r => r.Delete(entity));

        _mockRepo.Setup(r => r.SaveChangesAsync())
                 .Returns(Task.CompletedTask);

        var result = await _service.DeleteTeacher(1);

        Assert.True(result);
    }

    // -------------------------------------------------
    // TEST: UPDATE Teacher
    // -------------------------------------------------
    [Fact]
    public async Task UpdateTeacher_ShouldUpdate_WhenValid()
    {
        var updateDto = new TeacherCreateDTO
        {
            Name = "Nuevo nombre",
            Email = "nuevo@test.com",
            HireDate = DateTime.Now
        };

        var entity = new Teacher
        {
            Id = 1,
            Name = "Viejo",
            Email = "viejo@test.com",
            HireDate = DateTime.Now.AddYears(-1)
        };

        _mockRepo.Setup(r => r.GetByIdAsync(1))
                 .ReturnsAsync(entity);

        _mockRepo.Setup(r => r.SaveChangesAsync())
                 .Returns(Task.CompletedTask);

        var result = await _service.UpdateTeacher(updateDto, 1);

        Assert.Equal("Nuevo nombre", result.Name);
        Assert.Equal("nuevo@test.com", result.Email);
    }
}
