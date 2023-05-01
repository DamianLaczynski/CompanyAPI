using CompanyAPI.Entities;
using CompanyAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyAPI.Services
{
    public class AccountService : IAccountService
    {

        private readonly CompanyDbContext _context;

        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountService(CompanyDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _context = dbContext;
            _passwordHasher = passwordHasher;
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
        public string GenerateJwt(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }
    }
}
