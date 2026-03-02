namespace RodizioOrganistas.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
