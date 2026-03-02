using RodizioOrganistas.Domain.Enums;

namespace RodizioOrganistas.Domain.Entities;

public class OrganistAvailability
{
    public int Id { get; set; }
    public int OrganistId { get; set; }
    public Organist? Organist { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public ServiceType ServiceType { get; set; }
}
