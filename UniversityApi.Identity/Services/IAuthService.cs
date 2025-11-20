using Microsoft.AspNetCore.Identity;
using UniversityApi.Identity.DTOs;

namespace UniversityApi.Identity.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterStudentAsync(RegisterStudentDto dto);
        Task<IdentityResult> RegisterTeacherAsync(RegisterTeacherDto dto);
        Task<LoginResult> LoginAsync(LoginDto dto); 
    }
}
