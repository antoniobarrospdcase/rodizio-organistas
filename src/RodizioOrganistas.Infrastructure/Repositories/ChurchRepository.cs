using Microsoft.EntityFrameworkCore;
using RodizioOrganistas.Domain.Entities;
using RodizioOrganistas.Domain.Interfaces;
using RodizioOrganistas.Infrastructure.Data;

namespace RodizioOrganistas.Infrastructure.Repositories;

public class ChurchRepository(AppDbContext context) : Repository<Church>(context), IChurchRepository
{
    public async Task<int> CountAsync(string? term)
    {
        var query = Context.Churches.AsQueryable();
        if (!string.IsNullOrWhiteSpace(term)) query = query.Where(c => c.Name.Contains(term));
        return await query.CountAsync();
    }

    public override async Task<Church?> GetByIdAsync(int id)
        => await Context.Churches.Include(x => x.ServiceDays).FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IReadOnlyList<Church>> GetPagedAsync(string? term, int page, int pageSize)
    {
        var query = Context.Churches.Include(x => x.ServiceDays).AsQueryable();
        if (!string.IsNullOrWhiteSpace(term)) query = query.Where(c => c.Name.Contains(term));
        return await query.OrderBy(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
    }
}
