using RodizioOrganistas.Domain.Enums;

namespace RodizioOrganistas.Domain.Entities;

public class ScheduleAssignment
{
    public DateOnly Date { get; set; }
    public DayOfWeek DayOfWeek => Date.DayOfWeek;
    public ServiceType ServiceType { get; set; }
    public string OrganistName { get; set; } = string.Empty;
    public string? HalfHourOrganistName { get; set; }
}
