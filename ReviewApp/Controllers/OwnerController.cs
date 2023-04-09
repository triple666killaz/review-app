using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OwnerController : Controller
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository,IMapper mapper)
    {
        _ownerRepository = ownerRepository;
        _countryRepository = countryRepository;     
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
    public IActionResult GetOwners()
    {
        var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(owners);
    }

    [HttpGet("{ownerId}")]
    [ProducesResponseType(200, Type = typeof(Owner))]
    [ProducesResponseType(400)]
    public IActionResult GetOwner(int ownerId)
    {
        if (!_ownerRepository.OwnerExists(ownerId))
            return NotFound();
        
        var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(owner);
    }

    [HttpGet("{ownerId}/pokemons")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    public IActionResult GetOwnerPokemons(int ownerId)
    {
        if (!_ownerRepository.OwnerExists(ownerId))
            return NotFound();
        
        var pokemons = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetOwnerPokemons(ownerId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pokemons);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateOwner([FromQuery]int countryId, [FromBody]OwnerDto ownerCreate)
    {
        if (ownerCreate == null)
            return BadRequest(ModelState);

        var owner = _ownerRepository.GetOwners()
            .FirstOrDefault(o => o.LastName.Trim().ToUpper() == ownerCreate.LastName.Trim().ToUpper()
                                 && o.FirstName.Trim().ToUpper() == ownerCreate.FirstName.Trim().ToUpper());

        if (owner != null)
        {
            ModelState.AddModelError("", "Owner already exists");
            return StatusCode(422, ModelState);
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var ownerMap = _mapper.Map<Owner>(ownerCreate);

        ownerMap.Country = _countryRepository.GetCountry(countryId);

        if (!_ownerRepository.CreateOwner(ownerMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{ownerId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto updatedOwner)
    {
        if (updatedOwner == null)
            return BadRequest(ModelState);

        if (ownerId != updatedOwner.Id)
            return BadRequest(ModelState);

        if (!_ownerRepository.OwnerExists(ownerId))
            return NotFound();
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var ownerMap = _mapper.Map<Owner>(updatedOwner);

        if (!_ownerRepository.UpdateOwner(ownerMap))
        {
            ModelState.AddModelError("", "Somethhing went wrong while updating owner");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully updated");
    }
}