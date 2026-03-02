using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RodizioOrganistas.Domain.Entities;
using RodizioOrganistas.Domain.Enums;
using RodizioOrganistas.Domain.Interfaces;
using RodizioOrganistas.Web.Models;

namespace RodizioOrganistas.Web.Controllers;

[Authorize]
public class OrganistsController(IOrganistRepository organistRepository, IChurchRepository churchRepository, IUnitOfWork unitOfWork) : Controller
{
    public async Task<IActionResult> Index(int churchId, string? term, int page = 1)
    {
        const int pageSize = 10;
        ViewBag.Church = await churchRepository.GetByIdAsync(churchId);
        ViewBag.Total = await organistRepository.CountByChurchAsync(churchId, term);
        ViewBag.Page = page;
        ViewBag.PageSize = pageSize;
        var items = await organistRepository.GetByChurchPagedAsync(churchId, term, page, pageSize);
        return View(items);
    }

    public async Task<IActionResult> Create(int churchId) => View(await CreateBaseVm(churchId));

    [HttpPost]
    public async Task<IActionResult> Create(OrganistViewModel model)
    {
        if (!ModelState.IsValid) return View(await MergeDays(model));
        await organistRepository.AddAsync(Map(model));
        await unitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new { churchId = model.ChurchId });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var organist = await organistRepository.GetByIdAsync(id);
        if (organist is null) return NotFound();
        return View(await ToVm(organist));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(OrganistViewModel model)
    {
        if (!ModelState.IsValid) return View(await MergeDays(model));
        var organist = await organistRepository.GetByIdAsync(model.Id);
        if (organist is null) return NotFound();
        organist.Name = model.Name;
        organist.ShortName = model.ShortName;
        organist.Phone = model.Phone;
        organist.CanPlayYouthMeeting = model.CanPlayYouthMeeting;
        organist.CanPlayOfficialServices = model.CanPlayOfficialServices;
        organist.CanPlayHalfHour = model.CanPlayHalfHour;
        organist.Availabilities.Clear();
        foreach (var d in model.YouthDays) organist.Availabilities.Add(new OrganistAvailability { DayOfWeek = d, ServiceType = ServiceType.YouthMeeting });
        foreach (var d in model.OfficialDays) organist.Availabilities.Add(new OrganistAvailability { DayOfWeek = d, ServiceType = ServiceType.OfficialService });
        organistRepository.Update(organist);
        await unitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new { churchId = model.ChurchId });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id, int churchId)
    {
        var organist = await organistRepository.GetByIdAsync(id);
        if (organist is null) return NotFound();
        organistRepository.Remove(organist);
        await unitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new { churchId });
    }

    private async Task<OrganistViewModel> CreateBaseVm(int churchId)
    {
        var church = await churchRepository.GetByIdAsync(churchId) ?? throw new InvalidOperationException();
        return new OrganistViewModel
        {
            ChurchId = churchId,
            AllowedYouthDays = church.ServiceDays.Where(x => x.ServiceType == ServiceType.YouthMeeting).Select(x => x.DayOfWeek).ToList(),
            AllowedOfficialDays = church.ServiceDays.Where(x => x.ServiceType == ServiceType.OfficialService).Select(x => x.DayOfWeek).ToList()
        };
    }

    private Organist Map(OrganistViewModel model) => new()
    {
        ChurchId = model.ChurchId,
        Name = model.Name,
        ShortName = model.ShortName,
        Phone = model.Phone,
        CanPlayYouthMeeting = model.CanPlayYouthMeeting,
        CanPlayOfficialServices = model.CanPlayOfficialServices,
        CanPlayHalfHour = model.CanPlayHalfHour,
        Availabilities = model.YouthDays.Select(x => new OrganistAvailability { DayOfWeek = x, ServiceType = ServiceType.YouthMeeting })
            .Concat(model.OfficialDays.Select(x => new OrganistAvailability { DayOfWeek = x, ServiceType = ServiceType.OfficialService })).ToList()
    };

    private async Task<OrganistViewModel> ToVm(Organist organist)
    {
        var baseVm = await CreateBaseVm(organist.ChurchId);
        baseVm.Id = organist.Id;
        baseVm.Name = organist.Name;
        baseVm.ShortName = organist.ShortName;
        baseVm.Phone = organist.Phone;
        baseVm.CanPlayYouthMeeting = organist.CanPlayYouthMeeting;
        baseVm.CanPlayOfficialServices = organist.CanPlayOfficialServices;
        baseVm.CanPlayHalfHour = organist.CanPlayHalfHour;
        baseVm.YouthDays = organist.Availabilities.Where(a => a.ServiceType == ServiceType.YouthMeeting).Select(a => a.DayOfWeek).ToList();
        baseVm.OfficialDays = organist.Availabilities.Where(a => a.ServiceType == ServiceType.OfficialService).Select(a => a.DayOfWeek).ToList();
        return baseVm;
    }

    private async Task<OrganistViewModel> MergeDays(OrganistViewModel model)
    {
        var baseVm = await CreateBaseVm(model.ChurchId);
        model.AllowedYouthDays = baseVm.AllowedYouthDays;
        model.AllowedOfficialDays = baseVm.AllowedOfficialDays;
        return model;
    }
}
