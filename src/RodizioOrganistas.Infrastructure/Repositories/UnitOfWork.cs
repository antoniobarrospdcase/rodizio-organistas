using RodizioOrganistas.Domain.Interfaces;
using RodizioOrganistas.Infrastructure.Data;

namespace RodizioOrganistas.Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync() => context.SaveChangesAsync();
}
