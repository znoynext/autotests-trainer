namespace AutotestsTrainer.Web.Models;

public class AppUser
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public ICollection<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
}
