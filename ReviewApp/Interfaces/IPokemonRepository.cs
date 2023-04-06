﻿using System.Collections;
using ReviewApp.Models;

namespace ReviewApp.Interfaces;

public interface IPokemonRepository
{
    ICollection<Pokemon> GetPokemons();
    Pokemon GetPokemon(int id);
    Pokemon GetPokemon(string name);
    decimal GetPokemonRating(int id);
    bool PokemonExists(int id);
    ICollection<Review> GetPokemonReviews(int id);
    ICollection<Owner> GetPokemonOwners(int id);
}