using System.ComponentModel.DataAnnotations;

namespace TradingCompany.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter a valid username.")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 10 characters long.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter a valid password.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
