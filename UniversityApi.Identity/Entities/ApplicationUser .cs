using Microsoft.AspNetCore.Identity;

namespace UniversityApi.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}