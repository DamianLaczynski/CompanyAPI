using CompanyAPI.Entities;
using CompanyAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CompanyAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly CompanyDbContext _context;

        private readonly IPasswordHasher<User> _passwordHasher;

        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(CompanyDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _context = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public void RegisterUser(RegisterDto dto)
        {
            // Tworzymy nowy obiekt Contact na podstawie przekazanych danych
            var newContact = new User()
            {
                Email = dto.Email,
                Name = dto.Name,
                Surname = dto.Surname
            };

            // Haszujemy hasło użytkownika i zapisujemy je w bazie danych
            var hashedPassword = _passwordHasher.HashPassword(newContact, dto.Password);
            newContact.HashedPassword = hashedPassword;
            _context.Users.Add(newContact);
            _context.SaveChanges();
        }

        // Metoda służąca do generowania tokena JWT na podstawie danych logowania
        public string GenerateJwt(LoginDto loginDto)
        {
            // Sprawdzamy, czy użytkownik o podanym adresie email istnieje w bazie danych
            var user = _context.Users.FirstOrDefault(u => u.Email == loginDto.Email);

            if (user is null)
            {
                throw new Exception(message: "Incorrect email or password");
            }

            // Pobieramy rolę użytkownika z bazy danych

            // Sprawdzamy, czy podane hasło jest poprawne
            var result = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception(message: "Incorrect email or password");
            }

            // Tworzymy listę claimów potrzebnych do wygenerowania tokenu użytkownika
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}")
            };

            // Tworzymy klucz szyfrujący na podstawie ustawień autentykacji
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            // Tworzymy nowy token JWT na podstawie claimów i klucza szyfrującego
            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            // Tworzymy nowy handler tokena i zwracamy zaszyfrowany token w postaci ciągu znaków
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
