namespace AutotestsTrainer.Web.Models;

public class WorkItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "Open";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int OwnerUserId { get; set; }
    public AppUser? OwnerUser { get; set; }
}
