using CompanyAPI.Models;
using CompanyAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyAPI.Controllers
{
    public class AccountController : ControllerBase
    {

        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public ActionResult RegisterContact([FromBody] RegisterDto dto)
        {
            _accountService.RegisterUser(dto);
            return Ok(); //Zwracanie status 200 OK
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _accountService.GenerateJwt(dto); // Generowanie JWT dla danych z DTO
            return Ok(new
            {
                Token = token,
                Message = "Login Success"
            }); //Token zwracany w odpowiedzi
        }
    }
}
