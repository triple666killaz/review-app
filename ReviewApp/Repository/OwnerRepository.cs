using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository;

public class OwnerRepository : IOwnerRepository
{
    private readonly DataContext _context;

    public OwnerRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Owner> GetOwners()
    {
        return _context.Owners.OrderBy(o => o.Id).ToList();
    }

    public Owner GetOwner(int id)
    {
        return _context.Owners.FirstOrDefault(o => o.Id == id);
    }

    public bool OwnerExists(int id)
    {
        return _context.Owners.Any(o => o.Id == id);
    }

    public ICollection<Pokemon> GetOwnerPokemons(int ownerId)
    {
        return _context.PokemonOwners.Where(e => e.OwnerId == ownerId).Select(o => o.Pokemon).ToList();
    }

    public bool CreateOwner(Owner owner)
    {
        _context.Add(owner);
        return _context.SaveChanges() > 0;
    }

    public bool UpdateOwner(Owner owner)
    {
        _context.Update(owner);
        return _context.SaveChanges() > 0;
    }
}


