namespace TimeBinTracker.API.Models;

public class UpdateDay
{
    public required DateOnly Date { get; set; }
    public required string Name { get; set; }
    public required string Activity { get; set; }

    public string ToCsvLine() => $"{Date:yyyy-MM-dd},{Activity},{Name}";
}
