using ReviewApp.Models;

namespace ReviewApp.Interfaces;

public interface ICountryRepository
{
    ICollection<Country> GetCountries();
    Country GetCountry(int id);
    bool CountryExists(int id);
    ICollection<Owner> GetOwnersByCountry(int id);
}