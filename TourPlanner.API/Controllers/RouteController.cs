namespace TourPlanner.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourPlanner.BL;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RouteController : ControllerBase
{
    private readonly IRouteService _routeService;

    public RouteController(IRouteService routeService)
    {
        _routeService = routeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoute(
        [FromQuery] string from,
        [FromQuery] string to,
        [FromQuery] string transportType)
    {
        var route = await _routeService.GetRouteAsync(from, to, transportType);
        if (route is null)
            return BadRequest(new { error = "Could not calculate route" });

        return Ok(route);
    }
}
