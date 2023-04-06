using ReviewApp.Models;

namespace ReviewApp.Interfaces;

public interface IReviewerRepository
{
    ICollection<Reviewer> GetReviewers();
    Reviewer GetReviewer(int id);
    bool ReviewerExists(int id);
    ICollection<Review> GetReviewerReviews(int id);
}