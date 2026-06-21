namespace AutotestsTrainer.Web.Models;

public class WorkItemsPageViewModel
{
    public string UserName { get; set; } = string.Empty;
    public IReadOnlyList<WorkItemListItemViewModel> Items { get; set; } = Array.Empty<WorkItemListItemViewModel>();
    public WorkItemCreateViewModel Create { get; set; } = new();
}
