namespace TourPlanner.API.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourPlanner.BL;
using TourPlanner.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ToursController : ControllerBase
{
    private readonly ITourService _tourService;

    public ToursController(ITourService tourService)
    {
        _tourService = tourService;
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(claim!);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Tour>> GetAll()
    {
        return Ok(_tourService.GetAllByUserId(GetUserId()));
    }

    [HttpGet("{id:guid}")]
    public ActionResult<Tour> GetById(Guid id)
    {
        var tour = _tourService.GetById(id);
        if (tour is null || tour.UserId != GetUserId())
            return NotFound();

        return Ok(tour);
    }

    [HttpPost]
    public ActionResult<Tour> Create(Tour tour)
    {
        tour.UserId = GetUserId();
        var created = _tourService.Add(tour);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<Tour> Update(Guid id, Tour tour)
    {
        var existing = _tourService.GetById(id);
        if (existing is null || existing.UserId != GetUserId())
            return NotFound();

        tour.Id = id;
        tour.UserId = GetUserId();
        var updated = _tourService.Update(tour);
        if (updated is null)
            return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var existing = _tourService.GetById(id);
        if (existing is null || existing.UserId != GetUserId())
            return NotFound();

        _tourService.Delete(id);
        return NoContent();
    }
}
