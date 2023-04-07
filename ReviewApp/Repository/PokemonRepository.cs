using Microsoft.EntityFrameworkCore;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository;

public class PokemonRepository : IPokemonRepository
{
    private readonly DataContext _context;

    public PokemonRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Pokemon> GetPokemons()
    {
        return _context.Pokemon.OrderBy(p => p.Id).ToList();
    }

    public Pokemon GetPokemon(int id)
    {
        return _context.Pokemon.FirstOrDefault(p => p.Id == id);

    }

    public Pokemon GetPokemon(string name)
    {
        return _context.Pokemon.Where(p => p.Name == name).FirstOrDefault();
    }

    public decimal GetPokemonRating(int pokeId)
    {
        var reviews = _context.Reviews.Where(p => p.Pokemon.Id == pokeId);

        if (reviews.Count() <= 0)
            return 0;

        return (decimal)reviews.Sum(r => r.Rating) / reviews.Count(); 
    }

    public bool PokemonExists(int pokeId)
    {
        return _context.Pokemon.Any(p => p.Id == pokeId);
    }

   

    public ICollection<Review> GetPokemonReviews(int id)
    {
        return _context.Reviews.Where(r => r.Pokemon.Id == id).ToList();
    }

    public ICollection<Owner> GetPokemonOwners(int id)
    {
        return _context.PokemonOwners.Where(e => e.PokemonId == id).Select(p => p.Owner).ToList();
    }

    public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
    {
        var pokemonOwnerEntity = _context.Owners.FirstOrDefault(o => o.Id == ownerId);
        var pokemonCategoryEntity = _context.Categories.FirstOrDefault(c => c.Id == categoryId);

        var pokemonOwner = new PokemonOwner() { Owner = pokemonOwnerEntity, Pokemon = pokemon };
        _context.Add(pokemonOwner);
        
        var pokemonCategory = new PokemonCategory() { Category = pokemonCategoryEntity, Pokemon = pokemon };
        _context.Add(pokemonCategory);

        _context.Add(pokemon);

        return _context.SaveChanges() > 0;
    }
}