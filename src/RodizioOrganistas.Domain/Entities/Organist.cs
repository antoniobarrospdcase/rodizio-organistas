namespace RodizioOrganistas.Domain.Entities;

public class Organist
{
    public int Id { get; set; }
    public int ChurchId { get; set; }
    public Church? Church { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool CanPlayYouthMeeting { get; set; }
    public bool CanPlayOfficialServices { get; set; }
    public bool CanPlayHalfHour { get; set; }
    public ICollection<OrganistAvailability> Availabilities { get; set; } = new List<OrganistAvailability>();
}
