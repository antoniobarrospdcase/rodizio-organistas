using RodizioOrganistas.Domain.Entities;

namespace RodizioOrganistas.Domain.Interfaces;

public interface IUserRepository : IRepository<AppUser>
{
    Task<AppUser?> GetByUsernameAsync(string username);
}
