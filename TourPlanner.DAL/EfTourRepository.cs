namespace TourPlanner.DAL;

using TourPlanner.Models;

public class EfTourRepository : ITourRepository
{
    private readonly TourPlannerDbContext _db;

    public EfTourRepository(TourPlannerDbContext db)
    {
        _db = db;
    }

    public IEnumerable<Tour> GetAll()
    {
        return _db.Tours.ToList();
    }

    public IEnumerable<Tour> GetAllByUserId(Guid userId)
    {
        return _db.Tours.Where(t => t.UserId == userId).ToList();
    }

    public Tour? GetById(Guid id)
    {
        return _db.Tours.Find(id);
    }

    public Tour Add(Tour tour)
    {
        tour.Id = Guid.NewGuid();
        _db.Tours.Add(tour);
        _db.SaveChanges();
        return tour;
    }

    public Tour? Update(Tour tour)
    {
        var existing = _db.Tours.Find(tour.Id);
        if (existing is null) return null;

        existing.Name = tour.Name;
        existing.Description = tour.Description;
        existing.From = tour.From;
        existing.To = tour.To;
        existing.TransportType = tour.TransportType;
        existing.Distance = tour.Distance;
        existing.EstimatedTime = tour.EstimatedTime;
        existing.RouteImage = tour.RouteImage;
        existing.RouteCoordinates = tour.RouteCoordinates;
        _db.SaveChanges();
        return existing;
    }

    public bool Delete(Guid id)
    {
        var tour = _db.Tours.Find(id);
        if (tour is null) return false;

        _db.Tours.Remove(tour);
        _db.SaveChanges();
        return true;
    }
}
