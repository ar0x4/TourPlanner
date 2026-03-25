namespace TourPlanner.Models;

public class RouteInfo
{
    public double Distance { get; set; }
    public string EstimatedTime { get; set; } = string.Empty;
    public double[][] Coordinates { get; set; } = [];
}
