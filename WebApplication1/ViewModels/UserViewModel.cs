using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class UserViewModel
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
