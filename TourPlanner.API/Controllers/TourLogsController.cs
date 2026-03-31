namespace TourPlanner.API.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourPlanner.BL;
using TourPlanner.Models;

[ApiController]
[Route("api/tours/{tourId:guid}/logs")]
[Authorize]
public class TourLogsController : ControllerBase
{
    private readonly ITourLogService _tourLogService;
    private readonly ITourService _tourService;

    public TourLogsController(ITourLogService tourLogService, ITourService tourService)
    {
        _tourLogService = tourLogService;
        _tourService = tourService;
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(claim!);
    }

    [HttpGet]
    public ActionResult<IEnumerable<TourLog>> GetAll(Guid tourId)
    {
        var tour = _tourService.GetById(tourId);
        if (tour is null || tour.UserId != GetUserId())
            return NotFound();

        return Ok(_tourLogService.GetByTourId(tourId));
    }

    [HttpGet("{id:guid}")]
    public ActionResult<TourLog> GetById(Guid tourId, Guid id)
    {
        var tour = _tourService.GetById(tourId);
        if (tour is null || tour.UserId != GetUserId())
            return NotFound();

        var log = _tourLogService.GetById(id);
        if (log is null || log.TourId != tourId)
            return NotFound();

        return Ok(log);
    }

    [HttpPost]
    public ActionResult<TourLog> Create(Guid tourId, TourLog log)
    {
        var tour = _tourService.GetById(tourId);
        if (tour is null || tour.UserId != GetUserId())
            return NotFound();

        log.TourId = tourId;
        var created = _tourLogService.Add(log);
        return CreatedAtAction(nameof(GetById), new { tourId, id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<TourLog> Update(Guid tourId, Guid id, TourLog log)
    {
        var tour = _tourService.GetById(tourId);
        if (tour is null || tour.UserId != GetUserId())
            return NotFound();

        log.Id = id;
        log.TourId = tourId;
        var updated = _tourLogService.Update(log);
        if (updated is null)
            return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid tourId, Guid id)
    {
        var tour = _tourService.GetById(tourId);
        if (tour is null || tour.UserId != GetUserId())
            return NotFound();

        var log = _tourLogService.GetById(id);
        if (log is null || log.TourId != tourId)
            return NotFound();

        _tourLogService.Delete(id);
        return NoContent();
    }
}
