using System.ComponentModel.DataAnnotations;

namespace AutotestsTrainer.Web.Models;

public class WorkItemCreateViewModel
{
    [Required(ErrorMessage = "Введите название")]
    [StringLength(200, ErrorMessage = "Название не должно быть длиннее 200 символов")]
    [Display(Name = "Название")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Описание не должно быть длиннее 1000 символов")]
    [Display(Name = "Описание")]
    public string? Description { get; set; }
}
