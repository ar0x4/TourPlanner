namespace TourPlanner.Models;

public class TourLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TourId { get; set; }
    public DateTime DateTime { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int Difficulty { get; set; }
    public double TotalDistance { get; set; }
    public string TotalTime { get; set; } = string.Empty;
    public int Rating { get; set; }
}
