using System.Security.Claims;
using AutotestsTrainer.Web.Data;
using AutotestsTrainer.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutotestsTrainer.Web.Controllers;

[Authorize]
public class DashboardController(AppDbContext db) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        var userName = User.Identity?.Name ?? "user";

        var items = await db.WorkItems
            .Where(x => x.OwnerUserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        var model = new DashboardViewModel
        {
            UserName = userName,
            TotalItems = items.Count,
            OpenItems = items.Count(x => x.Status == "Open"),
            DoneItems = items.Count(x => x.Status == "Done"),
            RecentItems = items
                .Take(5)
                .Select(x => new WorkItemListItemViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt
                })
                .ToList()
        };

        return View(model);
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
