using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository;

public class ReviewerRepository : IReviewerRepository
{
    private readonly DataContext _context;

    public ReviewerRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Reviewer> GetReviewers()
    {
        return _context.Reviewers.OrderBy(r => r.Id).ToList();
    }

    public Reviewer GetReviewer(int id)
    {
        return _context.Reviewers.FirstOrDefault(r => r.Id == id);
    }

    public bool ReviewerExists(int id)
    {
        return _context.Reviewers.Any(r => r.Id == id);
    }

    public ICollection<Review> GetReviewerReviews(int id)
    {
        return _context.Reviews.Where(r => r.Reviewer.Id == id).ToList();
    }

    public bool CreateReviewer(Reviewer reviewer)
    {
        _context.Add(reviewer);
        return _context.SaveChanges() > 0;
    }
}