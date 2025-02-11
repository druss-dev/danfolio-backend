namespace DanfolioBackend.Models;

public class WorkHistory
{
    public int Id { get; set; }
    public string Company { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<TechStack> TechStacks { get; set; } = new();
}
