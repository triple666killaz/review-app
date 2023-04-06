using ReviewApp.Models;

namespace ReviewApp.Interfaces;

public interface IReviewRepository
{
    ICollection<Review> GetReviews();
    Review GetReview(int id);
    bool ReviewExists(int id);
}