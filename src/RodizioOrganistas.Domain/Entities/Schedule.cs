using RodizioOrganistas.Domain.Enums;

namespace RodizioOrganistas.Domain.Entities;

public class Schedule
{
    public int Id { get; set; }
    public int ChurchId { get; set; }
    public Church? Church { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public ServiceType ServiceType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int CreatedByUserId { get; set; }
    public AppUser? CreatedByUser { get; set; }
    public ICollection<ScheduleItem> Items { get; set; } = new List<ScheduleItem>();
}
