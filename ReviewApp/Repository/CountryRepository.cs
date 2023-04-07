using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository;

public class CountryRepository : ICountryRepository
{
    private readonly DataContext _context;

    public CountryRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Country> GetCountries()
    {
        return _context.Countries.OrderBy(c => c.Id).ToList();
    }

    public Country GetCountry(int id)
    {
        return _context.Countries.Where(c => c.Id == id).FirstOrDefault();
    }

    public bool CountryExists(int id)
    {
        return _context.Countries.Any(c => c.Id == id);
    }

    public ICollection<Owner> GetOwnersByCountry(int id)
    {
        return _context.Owners.Where(o => o.Country.Id == id).OrderBy(o => o.Country.Name).ToList();
    }

    public bool CreateCountry(Country country)
    {
        _context.Add(country);
        return _context.SaveChanges() > 0;
    }
}