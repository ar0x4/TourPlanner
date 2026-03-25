namespace TourPlanner.DAL;

using TourPlanner.Models;

public interface ITourRepository
{
    IEnumerable<Tour> GetAll();
    IEnumerable<Tour> GetAllByUserId(Guid userId);
    Tour? GetById(Guid id);
    Tour Add(Tour tour);
    Tour? Update(Tour tour);
    bool Delete(Guid id);
}
