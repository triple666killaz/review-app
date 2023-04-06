using ReviewApp.Models;

namespace ReviewApp.Interfaces;

public interface ICategoryRepository
{
    ICollection<Category> GetCategories();
    Category GetCategory(int id);
    ICollection<Pokemon> GetPokemonByCategoryId(int id);
    bool CategoryExists(int id);
}