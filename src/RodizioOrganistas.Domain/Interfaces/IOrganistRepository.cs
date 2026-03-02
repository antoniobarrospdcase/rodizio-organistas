using RodizioOrganistas.Domain.Entities;

namespace RodizioOrganistas.Domain.Interfaces;

public interface IOrganistRepository : IRepository<Organist>
{
    Task<IReadOnlyList<Organist>> GetByChurchPagedAsync(int churchId, string? term, int page, int pageSize);
    Task<int> CountByChurchAsync(int churchId, string? term);
    Task<IReadOnlyList<Organist>> GetByChurchAsync(int churchId);
}
