using System.ComponentModel.DataAnnotations;

namespace Tutorial_5and6.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }
    }
}