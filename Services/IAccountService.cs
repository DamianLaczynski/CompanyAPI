using CompanyAPI.Models;

namespace CompanyAPI.Services
{
    public interface IAccountService
    {
        public void RegisterUser(RegisterDto contactDto);

        public string GenerateJwt(LoginDto loginDto);
    }
}
