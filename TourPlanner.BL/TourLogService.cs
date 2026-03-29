namespace TourPlanner.BL;

using TourPlanner.DAL;
using TourPlanner.Models;

public class TourLogService : ITourLogService
{
    private readonly ITourLogRepository _repository;

    public TourLogService(ITourLogRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<TourLog> GetByTourId(Guid tourId) => _repository.GetByTourId(tourId);
    public TourLog? GetById(Guid id) => _repository.GetById(id);
    public TourLog Add(TourLog log) => _repository.Add(log);
    public TourLog? Update(TourLog log) => _repository.Update(log);
    public bool Delete(Guid id) => _repository.Delete(id);
}
