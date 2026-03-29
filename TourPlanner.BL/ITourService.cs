namespace TourPlanner.BL;

using TourPlanner.Models;

public interface ITourService
{
    IEnumerable<Tour> GetAll();
    IEnumerable<Tour> GetAllByUserId(Guid userId);
    Tour? GetById(Guid id);
    Tour Add(Tour tour);
    Tour? Update(Tour tour);
    bool Delete(Guid id);
}
