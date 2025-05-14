using System.ComponentModel.DataAnnotations;

namespace DemoWeb.DAL.ViewModels;

public class UserDetailViewModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public string? Surname { get; set; }
}
