using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RodizioOrganistas.Domain.Entities;
using RodizioOrganistas.Domain.Enums;
using RodizioOrganistas.Domain.Interfaces;
using RodizioOrganistas.Web.Models;

namespace RodizioOrganistas.Web.Controllers;

[Authorize]
public class ChurchesController(IChurchRepository churchRepository, IUnitOfWork unitOfWork) : Controller
{
    public async Task<IActionResult> Index(string? term, int page = 1)
    {
        const int pageSize = 10;
        ViewBag.Term = term;
        ViewBag.Page = page;
        ViewBag.Total = await churchRepository.CountAsync(term);
        ViewBag.PageSize = pageSize;
        var items = await churchRepository.GetPagedAsync(term, page, pageSize);
        return View(items);
    }

    public IActionResult Create() => View(new ChurchViewModel());

    [HttpPost]
    public async Task<IActionResult> Create(ChurchViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var church = Map(model);
        await churchRepository.AddAsync(church);
        await unitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var church = await churchRepository.GetByIdAsync(id);
        if (church is null) return NotFound();
        return View(ToVm(church));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ChurchViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var church = await churchRepository.GetByIdAsync(model.Id);
        if (church is null) return NotFound();
        church.Name = model.Name;
        church.City = model.City;
        church.OfficialOrganistsPerService = model.OfficialOrganistsPerService;
        church.ServiceDays.Clear();
        foreach (var d in model.YouthMeetingDays) church.ServiceDays.Add(new ChurchServiceDay { DayOfWeek = d, ServiceType = ServiceType.YouthMeeting });
        foreach (var d in model.OfficialServiceDays) church.ServiceDays.Add(new ChurchServiceDay { DayOfWeek = d, ServiceType = ServiceType.OfficialService });
        churchRepository.Update(church);
        await unitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var church = await churchRepository.GetByIdAsync(id);
        if (church is null) return NotFound();
        churchRepository.Remove(church);
        await unitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private static Church Map(ChurchViewModel model) => new()
    {
        Name = model.Name,
        City = model.City,
        OfficialOrganistsPerService = model.OfficialOrganistsPerService,
        ServiceDays = model.YouthMeetingDays.Select(d => new ChurchServiceDay { DayOfWeek = d, ServiceType = ServiceType.YouthMeeting })
            .Concat(model.OfficialServiceDays.Select(d => new ChurchServiceDay { DayOfWeek = d, ServiceType = ServiceType.OfficialService })).ToList()
    };

    private static ChurchViewModel ToVm(Church c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        City = c.City,
        OfficialOrganistsPerService = c.OfficialOrganistsPerService,
        YouthMeetingDays = c.ServiceDays.Where(x => x.ServiceType == ServiceType.YouthMeeting).Select(x => x.DayOfWeek).ToList(),
        OfficialServiceDays = c.ServiceDays.Where(x => x.ServiceType == ServiceType.OfficialService).Select(x => x.DayOfWeek).ToList()
    };
}
