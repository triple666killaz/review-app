using ReviewApp.Models;

namespace ReviewApp.Interfaces;

public interface IReviewRepository
{
    ICollection<Review> GetReviews();
    Review GetReview(int id);
    bool ReviewExists(int id);
    bool CreateReview(Review review);
    bool UpdateReview(Review review);
}