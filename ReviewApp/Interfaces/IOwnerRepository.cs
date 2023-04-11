using ReviewApp.Models;

namespace ReviewApp.Interfaces;

public interface IOwnerRepository
{
    ICollection<Owner> GetOwners();
    Owner GetOwner(int id);
    bool OwnerExists(int id);
    ICollection<Pokemon> GetOwnerPokemons(int id);
    bool CreateOwner(Owner owner);
    bool UpdateOwner(Owner owner);
    bool DeleteOwner(Owner owner);
}