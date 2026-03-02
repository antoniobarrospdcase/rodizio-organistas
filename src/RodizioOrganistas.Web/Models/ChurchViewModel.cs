using System.ComponentModel.DataAnnotations;

namespace RodizioOrganistas.Web.Models;

public class ChurchViewModel
{
    public int Id { get; set; }
    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;
    [Required, StringLength(120)]
    public string City { get; set; } = string.Empty;
    [Range(1,2)]
    public int OfficialOrganistsPerService { get; set; } = 1;
    [StringLength(100)]
    public string AdminUsername { get; set; } = string.Empty;
    [StringLength(100)]
    public string AdminPassword { get; set; } = string.Empty;
    public List<DayOfWeek> YouthMeetingDays { get; set; } = [];
    public List<DayOfWeek> OfficialServiceDays { get; set; } = [];
}
