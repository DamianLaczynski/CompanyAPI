using System.ComponentModel.DataAnnotations;

namespace CompanyAPI.Models
{
    public class RegisterDto
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmedPassword { get; set; }

    }
}
