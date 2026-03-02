using Microsoft.EntityFrameworkCore;
using RodizioOrganistas.Domain.Entities;
using RodizioOrganistas.Domain.Enums;
using RodizioOrganistas.Domain.Interfaces;
using RodizioOrganistas.Infrastructure.Data;

namespace RodizioOrganistas.Infrastructure.Repositories;

public class ScheduleRepository(AppDbContext context) : Repository<Schedule>(context), IScheduleRepository
{
    public override async Task<Schedule?> GetByIdAsync(int id)
        => await Context.Schedules.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IReadOnlyList<Schedule>> GetByChurchAsync(int churchId, ServiceType? type = null)
    {
        var q = Context.Schedules.Include(x => x.Items).Where(x => x.ChurchId == churchId);
        if (type.HasValue) q = q.Where(x => x.ServiceType == type.Value);
        return await q.OrderByDescending(x => x.CreatedAt).AsNoTracking().ToListAsync();
    }
}
