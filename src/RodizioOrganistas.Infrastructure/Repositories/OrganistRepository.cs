using Microsoft.EntityFrameworkCore;
using RodizioOrganistas.Domain.Entities;
using RodizioOrganistas.Domain.Interfaces;
using RodizioOrganistas.Infrastructure.Data;

namespace RodizioOrganistas.Infrastructure.Repositories;

public class OrganistRepository(AppDbContext context) : Repository<Organist>(context), IOrganistRepository
{
    public override async Task<Organist?> GetByIdAsync(int id)
        => await Context.Organists.Include(x => x.Availabilities).FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IReadOnlyList<Organist>> GetByChurchAsync(int churchId)
        => await Context.Organists.Include(x => x.Availabilities).Where(x => x.ChurchId == churchId).ToListAsync();

    public async Task<int> CountByChurchAsync(int churchId, string? term)
    {
        var query = Context.Organists.Where(x => x.ChurchId == churchId);
        if (!string.IsNullOrWhiteSpace(term)) query = query.Where(o => o.Name.Contains(term) || o.ShortName.Contains(term));
        return await query.CountAsync();
    }

    public async Task<IReadOnlyList<Organist>> GetByChurchPagedAsync(int churchId, string? term, int page, int pageSize)
    {
        var query = Context.Organists.Include(x => x.Availabilities).Where(x => x.ChurchId == churchId);
        if (!string.IsNullOrWhiteSpace(term)) query = query.Where(o => o.Name.Contains(term) || o.ShortName.Contains(term));
        return await query.OrderBy(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
    }
}
