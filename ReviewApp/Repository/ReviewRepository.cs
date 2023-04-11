using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository;

public class ReviewRepository : IReviewRepository
{
    private readonly DataContext _context;

    public ReviewRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Review> GetReviews()
    {
        return _context.Reviews.OrderBy(r => r.Id).ToList();
    }

    public Review GetReview(int id)
    {
        return _context.Reviews.FirstOrDefault(r => r.Id == id);
    }

    public bool ReviewExists(int id)
    {
        return _context.Reviews.Any(r => r.Id == id);
    }

    public bool CreateReview(Review review)
    {
        _context.Add(review);
        return _context.SaveChanges() > 0;
    }

    public bool UpdateReview(Review review)
    {
        _context.Update(review);
        return _context.SaveChanges() > 0;
    }

    public bool DeleteReview(Review review)
    {
        _context.Remove(review);
        return _context.SaveChanges() > 0;
    }

    public bool DeleteReviews(List<Review> reviews)
    {
        _context.RemoveRange(reviews);
        return _context.SaveChanges() > 0;
    }
}