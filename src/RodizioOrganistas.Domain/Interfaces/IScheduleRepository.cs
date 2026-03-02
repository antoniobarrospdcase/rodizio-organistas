using RodizioOrganistas.Domain.Entities;
using RodizioOrganistas.Domain.Enums;

namespace RodizioOrganistas.Domain.Interfaces;

public interface IScheduleRepository : IRepository<Schedule>
{
    Task<IReadOnlyList<Schedule>> GetByChurchAsync(int churchId, ServiceType? type = null);
}
