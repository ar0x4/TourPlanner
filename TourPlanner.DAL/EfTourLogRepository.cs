namespace TourPlanner.DAL;

using TourPlanner.Models;

public class EfTourLogRepository : ITourLogRepository
{
    private readonly TourPlannerDbContext _db;

    public EfTourLogRepository(TourPlannerDbContext db)
    {
        _db = db;
    }

    public IEnumerable<TourLog> GetByTourId(Guid tourId)
    {
        return _db.TourLogs.Where(l => l.TourId == tourId).ToList();
    }

    public TourLog? GetById(Guid id)
    {
        return _db.TourLogs.Find(id);
    }

    public TourLog Add(TourLog log)
    {
        log.Id = Guid.NewGuid();
        _db.TourLogs.Add(log);
        _db.SaveChanges();
        return log;
    }

    public TourLog? Update(TourLog log)
    {
        var existing = _db.TourLogs.Find(log.Id);
        if (existing is null) return null;

        existing.DateTime = log.DateTime;
        existing.Comment = log.Comment;
        existing.Difficulty = log.Difficulty;
        existing.TotalDistance = log.TotalDistance;
        existing.TotalTime = log.TotalTime;
        existing.Rating = log.Rating;
        _db.SaveChanges();
        return existing;
    }

    public bool Delete(Guid id)
    {
        var log = _db.TourLogs.Find(id);
        if (log is null) return false;

        _db.TourLogs.Remove(log);
        _db.SaveChanges();
        return true;
    }
}
