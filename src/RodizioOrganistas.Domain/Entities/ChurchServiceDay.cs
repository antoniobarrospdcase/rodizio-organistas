using RodizioOrganistas.Domain.Enums;

namespace RodizioOrganistas.Domain.Entities;

public class ChurchServiceDay
{
    public int Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public ServiceType ServiceType { get; set; }
    public int ChurchId { get; set; }
    public Church? Church { get; set; }
}
