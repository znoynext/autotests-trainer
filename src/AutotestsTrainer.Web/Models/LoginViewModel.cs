using System.ComponentModel.DataAnnotations;

namespace AutotestsTrainer.Web.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Введите логин")]
    [Display(Name = "Логин")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}
