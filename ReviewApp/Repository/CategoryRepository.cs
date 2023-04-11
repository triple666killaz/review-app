﻿using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly DataContext _context;

    public CategoryRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Category> GetCategories()
    {
        return _context.Categories.OrderBy(c => c.Name).ToList();
    }

    public Category GetCategory(int id)
    {
        return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
    }

    public ICollection<Pokemon> GetPokemonByCategoryId(int categoryId)
    {
        return _context.PokemonCategories.Where(e => e.CategoryId == categoryId).Select(c => c.Pokemon).ToList();
    }
    
    public bool CategoryExists(int id)
    {
        return _context.Categories.Any(c => c.Id == id);
    }

    public bool CreateCategory(Category category)
    {
        _context.Add(category);
        return _context.SaveChanges() > 0;
    }

    public bool UpdateCategory(Category category)
    {
        _context.Update(category);
        return _context.SaveChanges() > 0;
    }

    public bool DeleteCategory(Category category)
    {
        _context.Remove(category);
        return _context.SaveChanges() > 0;
    }
}