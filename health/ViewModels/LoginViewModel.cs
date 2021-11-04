using System.ComponentModel.DataAnnotations;

namespace health.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Login { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
