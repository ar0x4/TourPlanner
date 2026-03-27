namespace TourPlanner.DAL;

using TourPlanner.Models;

public class EfUserRepository : IUserRepository
{
    private readonly TourPlannerDbContext _db;

    public EfUserRepository(TourPlannerDbContext db)
    {
        _db = db;
    }

    public User? GetByEmail(string email)
    {
        return _db.Users.FirstOrDefault(u => u.Email == email);
    }

    public User Add(User user)
    {
        user.Id = Guid.NewGuid();
        _db.Users.Add(user);
        _db.SaveChanges();
        return user;
    }
}
