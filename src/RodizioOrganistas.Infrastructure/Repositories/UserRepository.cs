using Microsoft.EntityFrameworkCore;
using RodizioOrganistas.Domain.Entities;
using RodizioOrganistas.Domain.Interfaces;
using RodizioOrganistas.Infrastructure.Data;

namespace RodizioOrganistas.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : Repository<AppUser>(context), IUserRepository
{
    public async Task<AppUser?> GetByUsernameAsync(string username)
        => await Context.Users.FirstOrDefaultAsync(x => x.Username == username);
}
