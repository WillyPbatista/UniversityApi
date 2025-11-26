using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniversityApi.Identity.Entities;
using UniversityApi.Identity.Jwt;
using UniversityApi.Identity.DTOs;
using UniversityApi.Domain;
using UniversityApi.Infrastructure;

namespace UniversityApi.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtTokenGenerator _jwt;
        private readonly IStudentRepository _StudentRepository;
        private readonly ITeacherRepository _TeacherRepository;


        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            JwtTokenGenerator jwt,
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwt = jwt;
            _StudentRepository = studentRepository;
            _TeacherRepository = teacherRepository;
        }

        public async Task<IdentityResult> RegisterStudentAsync(RegisterStudentDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "The request body cannot be null.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is a required field", nameof(dto.Name));

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is a required field", nameof(dto.Email));

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.Name
            };

            var createResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded) return createResult;


            if (!await _roleManager.RoleExistsAsync("Student"))
                await _roleManager.CreateAsync(new IdentityRole("Student"));


            await _userManager.AddToRoleAsync(user, "Student");

            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                EnrollmentDate = DateTime.UtcNow,
                UserId = user.Id
            };

            await _StudentRepository.AddAsync(student);
            await _StudentRepository.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> RegisterTeacherAsync(RegisterTeacherDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "The request body cannot be null.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is a required field", nameof(dto.Name));

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is a required field", nameof(dto.Email));

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.Name
            };

            var createResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded) return createResult;

            if (!await _roleManager.RoleExistsAsync("Teacher"))
                await _roleManager.CreateAsync(new IdentityRole("Teacher"));

            await _userManager.AddToRoleAsync(user, "Teacher");

            var teacher = new Teacher
            {
                Name = dto.Name,
                Email = dto.Email,
                HireDate = DateTime.UtcNow,
                UserId = user.Id
            };

            await _TeacherRepository.AddAsync(teacher);
            await _TeacherRepository.SaveChangesAsync();

            return IdentityResult.Success;
        }

public async Task<LoginResult> LoginAsync(LoginDto dto)
{
    if (string.IsNullOrWhiteSpace(dto.Email))
        return new LoginResult { Success = false, Error = "Email is required." };

    if (string.IsNullOrWhiteSpace(dto.Password))
        return new LoginResult { Success = false, Error = "Password is required." };

    var user = await _userManager.FindByEmailAsync(dto.Email);
    if (user is null)
        return new LoginResult { Success = false, Error = "User not found." };

    if (await _userManager.IsLockedOutAsync(user))
        return new LoginResult { Success = false, Error = "User account is locked." };

    var signIn = await _signInManager.CheckPasswordSignInAsync(
        user,
        dto.Password,
        lockoutOnFailure: true 
    );

    if (!signIn.Succeeded)
        return new LoginResult { Success = false, Error = "Invalid credentials." };

    var roles = await _userManager.GetRolesAsync(user);

    if (roles.Count == 0)
        return new LoginResult { Success = false, Error = "User has no assigned role." };

    var extraClaims = new Dictionary<string, string>();

    var student = await _StudentRepository.GetByUserIdAsync(user.Id);
    if (student != null)
        extraClaims["StudentId"] = student.Id.ToString();

    var teacher = await _TeacherRepository.GetByUserIdAsync(user.Id);
    if (teacher != null)
        extraClaims["TeacherId"] = teacher.Id.ToString();

    string token;

    try
    {
        token = _jwt.GenerateToken(user, roles, extraClaims);
    }
    catch (Exception)
    {
        return new LoginResult { Success = false, Error = "Failed to generate authentication token." };
    }

    return new LoginResult
    {
        Success = true,
        Token = token
    };
}

    }
}
