using System.ComponentModel.DataAnnotations;

namespace RodizioOrganistas.Web.Models;

public class OrganistViewModel
{
    public int Id { get; set; }
    public int ChurchId { get; set; }
    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;
    [Required, StringLength(50)]
    public string ShortName { get; set; } = string.Empty;
    [Required, StringLength(20)]
    public string Phone { get; set; } = string.Empty;
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;
    [StringLength(100)]
    public string Password { get; set; } = string.Empty;
    public bool CanPlayYouthMeeting { get; set; }
    public bool CanPlayOfficialServices { get; set; }
    public bool CanPlayHalfHour { get; set; }
    public List<DayOfWeek> YouthDays { get; set; } = [];
    public List<DayOfWeek> OfficialDays { get; set; } = [];
    public List<DayOfWeek> AllowedYouthDays { get; set; } = [];
    public List<DayOfWeek> AllowedOfficialDays { get; set; } = [];
}
