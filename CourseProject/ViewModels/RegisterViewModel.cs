using System.ComponentModel.DataAnnotations;

namespace CourseProject.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введіть логін")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть ім'я")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть номер телефону")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть email")]
        [EmailAddress(ErrorMessage = "Некоректний email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть пароль")]
        [MinLength(6, ErrorMessage = "Пароль має містити мінімум 6 символів")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Повторіть пароль")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}