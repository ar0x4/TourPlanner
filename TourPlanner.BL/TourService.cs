namespace TourPlanner.BL;

using TourPlanner.DAL;
using TourPlanner.Models;

public class TourService : ITourService
{
    private readonly ITourRepository _repository;

    public TourService(ITourRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Tour> GetAll() => _repository.GetAll();
    public IEnumerable<Tour> GetAllByUserId(Guid userId) => _repository.GetAllByUserId(userId);
    public Tour? GetById(Guid id) => _repository.GetById(id);
    public Tour Add(Tour tour) => _repository.Add(tour);
    public Tour? Update(Tour tour) => _repository.Update(tour);
    public bool Delete(Guid id) => _repository.Delete(id);
}
