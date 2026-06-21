using System.Security.Claims;
using AutotestsTrainer.Web.Data;
using AutotestsTrainer.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutotestsTrainer.Web.Controllers;

[ApiController]
[Authorize]
[Route("api/workitems")]
public class WorkItemsApiController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetWorkItems()
    {
        var userId = GetUserId();
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

        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWorkItem(WorkItemCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var entity = new WorkItem
        {
            Title = model.Title.Trim(),
            Description = model.Description?.Trim(),
            Status = "Open",
            CreatedAt = DateTime.UtcNow,
            OwnerUserId = GetUserId()
        };

        db.WorkItems.Add(entity);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetWorkItems), new { id = entity.Id }, new
        {
            entity.Id,
            entity.Title,
            entity.Description,
            entity.Status,
            entity.CreatedAt
        });
    }

    [HttpPost("{id:int}/complete")]
    public async Task<IActionResult> CompleteWorkItem(int id)
    {
        var userId = GetUserId();
        var item = await db.WorkItems.SingleOrDefaultAsync(x => x.Id == id && x.OwnerUserId == userId);
        if (item is null)
        {
            return NotFound();
        }

        item.Status = "Done";
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("/api/me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            UserName = User.Identity?.Name,
            DisplayName = User.FindFirstValue("display_name")
        });
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
