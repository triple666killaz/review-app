using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountryController : Controller
{
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public CountryController(ICountryRepository countryRepository, IMapper mapper)
    {
        _countryRepository = countryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
    public IActionResult GetCountries()
    {
        var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(countries);
    }

    [HttpGet("{countryId}")]
    [ProducesResponseType(200, Type = typeof(Country))]
    [ProducesResponseType(400)]
    public IActionResult GetCountry(int countryId)
    {
        if (!_countryRepository.CountryExists(countryId))
            return NotFound();

        var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(country);
    }

    [HttpGet("{countryId}/owners")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
    [ProducesResponseType(400)]
    public IActionResult GetOwnersByCountry(int countryId)
    {
        if (!_countryRepository.CountryExists(countryId))
            return NotFound();

        var owners = _mapper.Map<List<OwnerDto>>(_countryRepository.GetOwnersByCountry(countryId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(owners);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCountry([FromBody]CountryDto countryCreate)
    {
        if (countryCreate == null)
            return BadRequest(ModelState);

        var country = _countryRepository.GetCountries()
            .FirstOrDefault(c => c.Name.Trim().ToUpper() == countryCreate.Name.Trim().ToUpper());

        if (country != null)
        {
            ModelState.AddModelError("", "Country already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var countryMap = _mapper.Map<Country>(countryCreate);

        if (!_countryRepository.CreateCountry(countryMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{countryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto updatedCountry)
    {
        if (updatedCountry == null)
            return BadRequest(ModelState);

        if (countryId != updatedCountry.Id)
            return BadRequest(ModelState);

        if (!_countryRepository.CountryExists(countryId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var countryMap = _mapper.Map<Country>(updatedCountry);

        if (!_countryRepository.UpdateCountry(countryMap))
        {
            ModelState.AddModelError("", "Something went wrong while updating country");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully updated");
    }

    [HttpDelete("{countryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteCountry(int countryId)
    {
        if (!_countryRepository.CountryExists(countryId))
            return NotFound();

        var country = _countryRepository.GetCountry(countryId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_countryRepository.GetOwnersByCountry(countryId).IsNullOrEmpty())
        {
            ModelState.AddModelError("", "Unable to delete due to connection with another entity");
            return StatusCode(500, ModelState);
        }

        if (!_countryRepository.DeleteCountry(country))
        {
            ModelState.AddModelError("", "Something went wrong while deleting category");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully deleted");
    }
    
}