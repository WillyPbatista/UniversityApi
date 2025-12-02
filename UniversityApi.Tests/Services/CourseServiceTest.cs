using Moq;
using Xunit;
using AutoMapper;
using UniversityApi.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;

public class CourseServiceTest
{
    private readonly Mock<IGenericRepository<Course>> _genricRepositoryMock;

    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly Mock<IMapper> _mockMapperMock;
    private readonly CourseService _courseService;

    public CourseServiceTest()
    {
        _genricRepositoryMock = new Mock<IGenericRepository<Course>>();
        _mockMapperMock = new Mock<IMapper>();
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _courseService = new CourseService(_mockMapperMock.Object, _genricRepositoryMock.Object, _courseRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateCourse_ShouldCreate_WhenValid()
    {
        var createDto = new CourseCreateDTO
        {
            Title = "Test1",
            Credits = "Test2",
            TeacherId = "1"
        };

        var entity = new Course
        {
            Id = 1,
            Title = "Test1",
            Credits = "Test2",
            TeacherId = 1
        };

        _genricRepositoryMock.Setup(r => r.GetAllAsync())
          .ReturnsAsync(new List<Course>());

        _mockMapperMock.Setup(m => m.Map<Course>(createDto))
                   .Returns(entity);

        _genricRepositoryMock.Setup(r => r.AddAsync(entity))
                 .Returns(Task.CompletedTask);

        _genricRepositoryMock.Setup(r => r.SaveChangesAsync())
                 .Returns(Task.CompletedTask);

        var result = await _courseService.CreateCourse(createDto);

        Assert.Equal("Test1", result.Title);
        Assert.Equal("1", result.TeacherId);
    }

    [Fact]
    public async Task GetCourse_ShouldReturnCourseDTO_WhenCourseExists()
    {
        var courseId = 1;

        var course = new Course { Id = courseId, Title = "Math" };
        var courseDto = new CourseDTO { Id = courseId, Title = "Math" };

        _courseRepositoryMock
            .Setup(r => r.GetById(courseId))
            .ReturnsAsync(course);

        _mockMapperMock
            .Setup(m => m.Map<CourseDTO>(course))
            .Returns(courseDto);

        var result = await _courseService.GetCourse(courseId);

        Assert.NotNull(result);
        Assert.Equal(courseId, result.Id);
        Assert.Equal("Math", result.Title);

        _courseRepositoryMock.Verify(r => r.GetById(courseId), Times.Once);
    }
    [Fact]
    public async Task GetCourse_ShouldThrowException_WhenCourseDoesNotExist()
    {

        var courseId = 1;

        _courseRepositoryMock
            .Setup(r => r.GetById(courseId))
            .ReturnsAsync((Course)null);

        await Assert.ThrowsAsync<ArgumentNullException>(() => _courseService.GetCourse(courseId));

        _courseRepositoryMock.Verify(r => r.GetById(courseId), Times.Once);
    }


    [Fact]
    public async Task DeleteCourse_ShouldDelete_WhenExists()
    {
        var courseId = 1;
        var course = new Course { Id = courseId, Title = "Math" };

        _genricRepositoryMock.Setup(r => r.GetByIdAsync(courseId)).ReturnsAsync(course);
        _genricRepositoryMock.Setup(r => r.Delete(course));
        _genricRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var result = await _courseService.DeleteCourse(courseId);
        Assert.True(result);
    }

    public async Task DeleteCourse_ShouldThrowExeption_WhenNotExist()
    {
        var courseId = 1;
        var course = new Course { Id = courseId, Title = "Math" };

        _genricRepositoryMock.Setup(r => r.GetByIdAsync(courseId)).ReturnsAsync((Course)null);

        await Assert.ThrowsAsync<ArgumentNullException>(() => _courseService.DeleteCourse(courseId));

    }

    
}