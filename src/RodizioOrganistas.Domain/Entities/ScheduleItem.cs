namespace RodizioOrganistas.Domain.Entities;

public class ScheduleItem
{
    public int Id { get; set; }
    public int ScheduleId { get; set; }
    public Schedule? Schedule { get; set; }
    public DateOnly Date { get; set; }
    public string OrganistName { get; set; } = string.Empty;
    public string? HalfHourOrganistName { get; set; }
}
