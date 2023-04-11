﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : Controller
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
    public IActionResult GetCategories()
    {
        var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(categories);
    }

    [HttpGet("{categoryId}")]
    [ProducesResponseType(200, Type = typeof(Category))]
    [ProducesResponseType(400)]
    public IActionResult GetCategory(int categoryId)
    {
        if (!_categoryRepository.CategoryExists(categoryId))
            return NotFound();

        var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(category);
    }

    [HttpGet("{categoryId}/pokemons")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonByCategoryId(int categoryId)
    {
        if (!_categoryRepository.CategoryExists(categoryId))
            return NotFound();

        var pokemons = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonByCategoryId(categoryId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pokemons);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCategory([FromBody]CategoryDto categoryCreate)
    {
        if (categoryCreate == null)
            return BadRequest(ModelState);

        var category = _categoryRepository
            .GetCategories()
            .FirstOrDefault(c => c.Name.Trim().ToUpper() ==
                                 categoryCreate.Name.Trim().ToUpper());

        if (category != null)
        {
            ModelState.AddModelError("", "Category already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var categoryMap = _mapper.Map<Category>(categoryCreate);

        if (!_categoryRepository.CreateCategory(categoryMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{categoryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateCategory(int categoryId, [FromBody]CategoryDto updatedCategory)
    {
        if (updatedCategory == null)
            return BadRequest(ModelState);

        if (categoryId != updatedCategory.Id)
            return BadRequest(ModelState);

        if (!_categoryRepository.CategoryExists(categoryId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var categoryMap = _mapper.Map<Category>(updatedCategory);

        if (!_categoryRepository.UpdateCategory(categoryMap))
        {
            ModelState.AddModelError("", "Something went wrong while updating category");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{categoryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteCategory(int categoryId)
    {
        if (!_categoryRepository.CategoryExists(categoryId))
            return NotFound();

        var category = _categoryRepository.GetCategory(categoryId);
        
        if (!_categoryRepository.GetPokemonByCategoryId(categoryId).IsNullOrEmpty())
        {
            ModelState.AddModelError("", "Unable to delete due to connection with another entity");
            return StatusCode(500, ModelState);
        }
        
        if (!_categoryRepository.DeleteCategory(category))
        {
            ModelState.AddModelError("", "Something went wrong while deleting category");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully deleted");
    }
}