namespace AutotestsTrainer.Web.Models;

public class DashboardViewModel
{
    public string UserName { get; set; } = string.Empty;
    public int TotalItems { get; set; }
    public int OpenItems { get; set; }
    public int DoneItems { get; set; }
    public IReadOnlyList<WorkItemListItemViewModel> RecentItems { get; set; } = Array.Empty<WorkItemListItemViewModel>();
}
