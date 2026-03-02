using System.ComponentModel.DataAnnotations;

namespace RodizioOrganistas.Web.Models;

public class ChangePasswordViewModel
{
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(NewPassword), ErrorMessage = "A confirmação não confere.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
