using System.Security.Claims;
using AutotestsTrainer.Web.Data;
using AutotestsTrainer.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutotestsTrainer.Web.Controllers;

[Authorize]
public class WorkItemsController(AppDbContext db) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await LoadModelAsync());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(WorkItemsPageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", await LoadModelAsync(model.Create ?? new WorkItemCreateViewModel()));
        }

        db.WorkItems.Add(new WorkItem
        {
            Title = model.Create.Title.Trim(),
            Description = model.Create.Description?.Trim(),
            Status = "Open",
            CreatedAt = DateTime.UtcNow,
            OwnerUserId = GetUserId()
        });

        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(int id)
    {
        var userId = GetUserId();
        var item = await db.WorkItems.SingleOrDefaultAsync(x => x.Id == id && x.OwnerUserId == userId);
        if (item is not null)
        {
            item.Status = "Done";
            await db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<WorkItemsPageViewModel> LoadModelAsync(WorkItemCreateViewModel? create = null)
    {
        var userId = GetUserId();
        var userName = User.Identity?.Name ?? "user";

        var items = await db.WorkItems
            .Where(x => x.OwnerUserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new WorkItemListItemViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Status = x.Status,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();

        return new WorkItemsPageViewModel
        {
            UserName = userName,
            Items = items,
            Create = create ?? new WorkItemCreateViewModel()
        };
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
