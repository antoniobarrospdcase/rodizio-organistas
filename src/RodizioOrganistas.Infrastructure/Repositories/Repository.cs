using Microsoft.EntityFrameworkCore;
using RodizioOrganistas.Domain.Interfaces;
using RodizioOrganistas.Infrastructure.Data;

namespace RodizioOrganistas.Infrastructure.Repositories;

public class Repository<T>(AppDbContext context) : IRepository<T> where T : class
{
    protected readonly AppDbContext Context = context;
    public async Task AddAsync(T entity) => await Context.Set<T>().AddAsync(entity);
    public async Task<IReadOnlyList<T>> GetAllAsync() => await Context.Set<T>().AsNoTracking().ToListAsync();
    public virtual async Task<T?> GetByIdAsync(int id) => await Context.Set<T>().FindAsync(id);
    public void Remove(T entity) => Context.Set<T>().Remove(entity);
    public void Update(T entity) => Context.Set<T>().Update(entity);
}
