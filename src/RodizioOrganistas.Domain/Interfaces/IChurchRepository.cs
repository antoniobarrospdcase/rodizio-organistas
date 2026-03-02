using RodizioOrganistas.Domain.Entities;

namespace RodizioOrganistas.Domain.Interfaces;

public interface IChurchRepository : IRepository<Church>
{
    Task<IReadOnlyList<Church>> GetPagedAsync(string? term, int page, int pageSize);
    Task<int> CountAsync(string? term);
}
