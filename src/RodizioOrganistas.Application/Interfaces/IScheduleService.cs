using RodizioOrganistas.Domain.Entities;
using RodizioOrganistas.Domain.Enums;

namespace RodizioOrganistas.Application.Interfaces;

public interface IScheduleService
{
    Task<IReadOnlyList<ScheduleAssignment>> GenerateAsync(int churchId, DateOnly startDate, DateOnly endDate, ServiceType serviceType);
}
