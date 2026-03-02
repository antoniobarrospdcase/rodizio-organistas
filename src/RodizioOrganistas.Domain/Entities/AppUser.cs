using RodizioOrganistas.Domain.Enums;

namespace RodizioOrganistas.Domain.Entities;

public class AppUser
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public int? ChurchId { get; set; }
    public Church? Church { get; set; }
    public int? OrganistId { get; set; }
    public Organist? Organist { get; set; }
}
