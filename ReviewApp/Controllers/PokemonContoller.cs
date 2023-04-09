using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonController : Controller
{
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IMapper _mapper;
    
    public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
    {
        _pokemonRepository = pokemonRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    public IActionResult GetPokemons()
    {
        var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pokemons);
    }

    [HttpGet("{pokemonId}")]
    [ProducesResponseType(200, Type = typeof(Pokemon))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemon(int pokemonId)
    {
        if (!_pokemonRepository.PokemonExists(pokemonId))
            return NotFound();

        var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokemonId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pokemon);
    }
    
    [HttpGet("{pokemonId}/rating")]
    [ProducesResponseType(200, Type = typeof(decimal))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonRating(int pokemonId)
    {
        if (!_pokemonRepository.PokemonExists(pokemonId))
            return NotFound();

        var rating = _pokemonRepository.GetPokemonRating(pokemonId);
        
        if (!ModelState.IsValid)
            return BadRequest();
        
        return Ok(Math.Round(rating, 2));
    }

    [HttpGet("{pokemonId}/reviews")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonReviews(int pokemonId)
    {
        if (!_pokemonRepository.PokemonExists(pokemonId))
            return NotFound();

        var reviews = _mapper.Map<List<ReviewDto>>(_pokemonRepository.GetPokemonReviews(pokemonId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(reviews);
    }
    
    [HttpGet("{pokemonId}/owners")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonOwners(int pokemonId)
    {
        if (!_pokemonRepository.PokemonExists(pokemonId))
            return NotFound();

        var owners = _mapper.Map<List<OwnerDto>>(_pokemonRepository.GetPokemonOwners(pokemonId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(owners);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreatePokemon([FromQuery]int ownerId, [FromQuery]int categoryId, [FromBody]PokemonDto pokemonCreate)
    {
        if (pokemonCreate == null)
            return BadRequest(ModelState);

        var pokemon = _pokemonRepository.GetPokemons().FirstOrDefault(p => p.Name.Trim().ToUpper()
                                                                            == pokemonCreate.Name.Trim().ToUpper());

        if (pokemon != null)
        {
            ModelState.AddModelError("", "Pokemon already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

        if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{pokemonId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdatePokemon(int pokemonId, [FromBody]PokemonDto updatedPokemon)
    {
        if (updatedPokemon == null)
            return BadRequest(ModelState);

        if (pokemonId != updatedPokemon.Id)
            return BadRequest(ModelState);

        if (!_pokemonRepository.PokemonExists(pokemonId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var pokemonMap = _mapper.Map<Pokemon>(updatedPokemon);

        if (!_pokemonRepository.UpdatePokemon(pokemonMap))
        {
            ModelState.AddModelError("", "Something went wrong while updating pokemon");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully updated");
    }
}
