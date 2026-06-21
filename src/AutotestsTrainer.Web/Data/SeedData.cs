using AutotestsTrainer.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutotestsTrainer.Web.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        await db.Database.EnsureCreatedAsync();

        var hasher = new PasswordHasher<AppUser>();

        var admin = await db.Users.SingleOrDefaultAsync(x => x.UserName == "admin");
        if (admin is null)
        {
            admin = new AppUser
            {
                UserName = "admin",
                DisplayName = "Admin"
            };
            db.Users.Add(admin);
        }
        else
        {
            admin.DisplayName = "Admin";
        }

        admin.PasswordHash = hasher.HashPassword(admin, "admin");

        if (!await db.WorkItems.AnyAsync(x => x.OwnerUserId == admin.Id && x.Title == "Проверить авторизацию"))
        {
            db.WorkItems.AddRange(
                new WorkItem
                {
                    Title = "Проверить авторизацию",
                    Description = "Открыть защищенную страницу после логина.",
                    Status = "Open",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    OwnerUser = admin
                },
                new WorkItem
                {
                    Title = "Сделать API-запрос",
                    Description = "Получить список задач пользователя через /api/workitems.",
                    Status = "Done",
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    OwnerUser = admin
                }
            );
        }

        await db.SaveChangesAsync();
    }
}
