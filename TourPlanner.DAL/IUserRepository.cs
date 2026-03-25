namespace TourPlanner.DAL;

using TourPlanner.Models;

public interface IUserRepository
{
    User? GetByEmail(string email);
    User Add(User user);
}
