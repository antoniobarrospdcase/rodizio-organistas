using RodizioOrganistas.Application.Interfaces;
using RodizioOrganistas.Domain.Entities;
using RodizioOrganistas.Domain.Enums;
using RodizioOrganistas.Domain.Interfaces;

namespace RodizioOrganistas.Application.Services;

public class ScheduleService(IChurchRepository churchRepository, IOrganistRepository organistRepository) : IScheduleService
{
    public async Task<IReadOnlyList<ScheduleAssignment>> GenerateAsync(int churchId, DateOnly startDate, DateOnly endDate, ServiceType serviceType)
    {
        var church = await churchRepository.GetByIdAsync(churchId) ?? throw new InvalidOperationException("Igreja não encontrada.");
        var validDays = church.ServiceDays.Where(x => x.ServiceType == serviceType).Select(x => x.DayOfWeek).ToHashSet();
        var candidates = (await organistRepository.GetByChurchAsync(churchId))
            .Where(o => serviceType == ServiceType.YouthMeeting ? o.CanPlayYouthMeeting : o.CanPlayOfficialServices)
            .ToList();

        if (candidates.Count == 0 || validDays.Count == 0)
        {
            return [];
        }

        var rotationIndex = 0;
        var halfHourIndex = 0;
        var result = new List<ScheduleAssignment>();

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if (!validDays.Contains(date.DayOfWeek)) continue;
            var available = candidates.Where(o => o.Availabilities.Any(a => a.ServiceType == serviceType && a.DayOfWeek == date.DayOfWeek)).ToList();
            if (available.Count == 0) continue;

            var main = available[rotationIndex % available.Count];
            rotationIndex++;

            string? halfHour = null;
            if (serviceType == ServiceType.OfficialService)
            {
                var halfHourCandidates = available.Where(x => x.CanPlayHalfHour).ToList();
                if (halfHourCandidates.Count > 0)
                {
                    halfHour = halfHourCandidates[halfHourIndex % halfHourCandidates.Count].ShortName;
                    halfHourIndex++;
                }
            }

            result.Add(new ScheduleAssignment
            {
                Date = date,
                ServiceType = serviceType,
                OrganistName = main.ShortName,
                HalfHourOrganistName = halfHour
            });

            if (serviceType == ServiceType.OfficialService && church.OfficialOrganistsPerService == 2 && available.Count > 1)
            {
                var second = available[rotationIndex % available.Count];
                rotationIndex++;
                result.Add(new ScheduleAssignment
                {
                    Date = date,
                    ServiceType = serviceType,
                    OrganistName = second.ShortName,
                    HalfHourOrganistName = null
                });
            }
        }

        return result;
    }
}
