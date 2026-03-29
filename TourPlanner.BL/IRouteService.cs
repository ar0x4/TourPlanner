namespace TourPlanner.BL;

using TourPlanner.Models;

public interface IRouteService
{
    Task<RouteInfo?> GetRouteAsync(string from, string to, string transportType);
}
