using System.ComponentModel.DataAnnotations;

namespace CourseProject.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введіть логін або email")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть пароль")]
        public string Password { get; set; } = string.Empty;
    }
}