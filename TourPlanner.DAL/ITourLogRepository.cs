namespace TourPlanner.DAL;

using TourPlanner.Models;

public interface ITourLogRepository
{
    IEnumerable<TourLog> GetByTourId(Guid tourId);
    TourLog? GetById(Guid id);
    TourLog Add(TourLog log);
    TourLog? Update(TourLog log);
    bool Delete(Guid id);
}
