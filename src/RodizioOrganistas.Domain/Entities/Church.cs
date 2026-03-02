namespace RodizioOrganistas.Domain.Entities;

public class Church
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int OfficialOrganistsPerService { get; set; } = 1;
    public ICollection<ChurchServiceDay> ServiceDays { get; set; } = new List<ChurchServiceDay>();
    public ICollection<Organist> Organists { get; set; } = new List<Organist>();
}
