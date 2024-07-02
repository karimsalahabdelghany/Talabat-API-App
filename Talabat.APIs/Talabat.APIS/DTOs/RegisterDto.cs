using System.ComponentModel.DataAnnotations;

namespace Talabat.APIS.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string DisplayName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^amp;*()_+]).*$",
            ErrorMessage = "Password must Contains 1 Uppercase ,1 lowecase , 1digit ,i special charcter")]
        public string Password { get; set; }


    }
}
