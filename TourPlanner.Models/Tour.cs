namespace TourPlanner.Models;

public class Tour
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public string TransportType { get; set; } = string.Empty;
    public double Distance { get; set; }
    public string EstimatedTime { get; set; } = string.Empty;
    public string RouteImage { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public string RouteCoordinates { get; set; } = string.Empty;
}
