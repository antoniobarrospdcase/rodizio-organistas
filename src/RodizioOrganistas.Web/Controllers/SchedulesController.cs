using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RodizioOrganistas.Application.Interfaces;
using RodizioOrganistas.Domain.Interfaces;
using RodizioOrganistas.Web.Models;

namespace RodizioOrganistas.Web.Controllers;

[Authorize]
public class SchedulesController(IScheduleService scheduleService, IChurchRepository churchRepository) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.Churches = await churchRepository.GetAllAsync();
        return View(new ScheduleFilterViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Index(ScheduleFilterViewModel model)
    {
        ViewBag.Churches = await churchRepository.GetAllAsync();
        if (!ModelState.IsValid) return View(model);
        model.Results = await scheduleService.GenerateAsync(model.ChurchId, model.StartDate, model.EndDate, model.ServiceType);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Print(ScheduleFilterViewModel model)
    {
        model.Results = await scheduleService.GenerateAsync(model.ChurchId, model.StartDate, model.EndDate, model.ServiceType);
        return View(model);
    }
}
