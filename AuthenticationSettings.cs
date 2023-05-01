namespace CompanyAPI
{
    public class AuthenticationSettings
    {
        //pola potrzebne do wygenerowania tokenu Jwt
        public string JwtKey { get; set; }

        public int JwtExpireDays { get; set; }

        public string JwtIssuer { get; set; }
    }
}
