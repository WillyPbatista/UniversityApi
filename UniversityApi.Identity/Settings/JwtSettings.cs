namespace UniversityApi.Identity.Jwt
{
    public class JwtSettings
    {
        public string Key { get; set; } = null!;             
        public string Issuer { get; set; } = "UniversityApi";
        public string Audience { get; set; } = "UniversityApiClients";
        public int ExpirationMinutes { get; set; } = 60;
    }
}
