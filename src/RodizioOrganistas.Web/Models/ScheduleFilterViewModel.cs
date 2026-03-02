using System.ComponentModel.DataAnnotations;
using RodizioOrganistas.Domain.Entities;
using RodizioOrganistas.Domain.Enums;

namespace RodizioOrganistas.Web.Models;

public class ScheduleFilterViewModel
{
    [Required]
    public int ChurchId { get; set; }
    [Required]
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    [Required]
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddDays(30));
    [Required]
    public ServiceType ServiceType { get; set; }
    public IReadOnlyList<ScheduleAssignment> Results { get; set; } = [];
}
