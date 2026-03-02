using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RodizioOrganistas.Application.Interfaces;
using RodizioOrganistas.Domain.Entities;
using RodizioOrganistas.Domain.Enums;
using RodizioOrganistas.Domain.Interfaces;
using RodizioOrganistas.Web.Models;

namespace RodizioOrganistas.Web.Controllers;

[Authorize]
public class SchedulesController(IScheduleService scheduleService, IScheduleRepository scheduleRepository, IChurchRepository churchRepository, IUnitOfWork unitOfWork) : Controller
{
    public async Task<IActionResult> Index()
    {
        var model = new ScheduleFilterViewModel();
        model.ChurchId = GetCurrentChurchIdOrDefault();
        await FillViewBagChurches();
        if (model.ChurchId > 0)
            model.SavedSchedules = await scheduleRepository.GetByChurchAsync(model.ChurchId);
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRole.MasterAdmin)+","+nameof(UserRole.ChurchAdmin))]
    public async Task<IActionResult> Index(ScheduleFilterViewModel model)
    {
        if (!CanAccessChurch(model.ChurchId)) return Forbid();
        await FillViewBagChurches();
        if (!ModelState.IsValid) return View(model);

        model.Results = await scheduleService.GenerateAsync(model.ChurchId, model.StartDate, model.EndDate, model.ServiceType);

        var schedule = new Schedule
        {
            ChurchId = model.ChurchId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            ServiceType = model.ServiceType,
            CreatedByUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
            Items = model.Results.Select(x => new ScheduleItem
            {
                Date = x.Date,
                OrganistName = x.OrganistName,
                HalfHourOrganistName = x.HalfHourOrganistName
            }).ToList()
        };

        await scheduleRepository.AddAsync(schedule);
        await unitOfWork.SaveChangesAsync();
        model.SavedScheduleId = schedule.Id;
        model.SavedSchedules = await scheduleRepository.GetByChurchAsync(model.ChurchId, model.ServiceType);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var schedule = await scheduleRepository.GetByIdAsync(id);
        if (schedule is null || !CanAccessChurch(schedule.ChurchId)) return Forbid();
        return View(schedule);
    }

    [HttpGet]
    public async Task<IActionResult> Print(int id)
    {
        var schedule = await scheduleRepository.GetByIdAsync(id);
        if (schedule is null || !CanAccessChurch(schedule.ChurchId)) return Forbid();
        return View(schedule);
    }

    private bool CanAccessChurch(int churchId)
    {
        if (User.IsInRole(nameof(UserRole.MasterAdmin))) return true;
        var claim = User.FindFirstValue("ChurchId");
        return int.TryParse(claim, out var c) && c == churchId;
    }

    private int GetCurrentChurchIdOrDefault()
    {
        if (User.IsInRole(nameof(UserRole.MasterAdmin))) return 0;
        return int.TryParse(User.FindFirstValue("ChurchId"), out var c) ? c : 0;
    }

    private async Task FillViewBagChurches()
    {
        if (User.IsInRole(nameof(UserRole.MasterAdmin)))
            ViewBag.Churches = await churchRepository.GetAllAsync();
        else
        {
            var c = await churchRepository.GetByIdAsync(GetCurrentChurchIdOrDefault());
            ViewBag.Churches = c is null ? new List<Church>() : new List<Church> { c };
        }
    }
}
